using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security;
using Terminal.Gui;

namespace CSLibreCell.Internal
{
    /// <summary>
    /// Helper class to load the configuration, providing defaults as necessary.
    /// </summary>
    internal class ConfigurationLoader
    {
        private const string ConfigFilename = "config.json";
        private const string JourneyDefaultFilename = "journey.cslibrecell";

        private static readonly Dictionary<string, Key> Dictionary = new Dictionary<string, Key>
        {
            { "<F1>", Key.F1 },
            { "<F2>", Key.F2 },
            { "<F3>", Key.F3 },
            { "<F4>", Key.F4 },
            { "<F5>", Key.F5 },
            { "<F6>", Key.F6 },
            { "<F7>", Key.F7 },
            { "<F8>", Key.F8 },
            { "<F9>", Key.F9 },
            { "<F10>", Key.F10 },
            { "<F11>", Key.F11 },
            { "<F12>", Key.F12 }
        };

        private readonly DirectoryInfo appDir;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationLoader"/> class.
        /// </summary>
        internal ConfigurationLoader()
        {
            this.appDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CSLibreCell"));
        }

        /// <summary>
        /// Reads the configuration from disks and adds defaults as necessary.
        /// </summary>
        /// <returns>A filled <see cref="Configuration"/> instance.</returns>
        internal (Configuration, List<string>) Read()
        {
            var log = new List<string>();

            var configFilepath = Path.Combine(appDir.FullName, ConfigFilename);
            var configFileinfo = new FileInfo(configFilepath);

            if (!configFileinfo.Exists)
            {
                log.Add($"{configFileinfo.FullName} does not exist");
                return (this.CreateDefaultConfiguration(), log);
            }

            var content = File.ReadAllText(configFileinfo.FullName);

            if (string.IsNullOrWhiteSpace(content))
            {
                log.Add($"{configFileinfo.FullName} does not have content");
                return (this.CreateDefaultConfiguration(), log);
            }

            try
            {
                var persistedConfiguration = JsonConvert.DeserializeObject<PersistedConfiguration>(content);

                var journeyFileinfo = IsValidPath(persistedConfiguration.JourneyPath)
                    ? new FileInfo(persistedConfiguration.JourneyPath)
                    : this.CreateDefaultJourneyFileinfo();

                log.Add($"using journey storage file {journeyFileinfo.FullName}");

                CultureInfo uiCulture = null;
                try
                {
                    if (!string.IsNullOrWhiteSpace(persistedConfiguration.UiCulture))
                    {
                        uiCulture = new CultureInfo(persistedConfiguration.UiCulture);
                        log.Add($"'{uiCulture.EnglishName}' parsed from {persistedConfiguration.UiCulture}");
                    }
                }
                catch (CultureNotFoundException)
                {
                    log.Add($"'{persistedConfiguration.UiCulture}' does not parse to a C# culture");
                }

                var configuration = new Configuration(journeyFileinfo, uiCulture);
                this.ConfigureKeys(persistedConfiguration, log, configuration);

                return (configuration, log);
            }
            catch (JsonException jex)
            {
                log.Add($"JSON deserialization exception for '{content}': {jex.Message} {jex.StackTrace}");
                return (this.CreateDefaultConfiguration(), log);
            }
        }

        private static bool IsValidPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) { return false; }
            bool status = false;

            try
            {
                var result = Path.GetFullPath(path);
                status = true;
            }
            catch (ArgumentException) { }
            catch (SecurityException) { }
            catch (NotSupportedException) { }
            catch (PathTooLongException) { }

            return status;
        }

        private Configuration CreateDefaultConfiguration()
        {
            FileInfo journeyFileinfo = CreateDefaultJourneyFileinfo();

            return new Configuration(journeyFileinfo, uiCulture: null);
        }

        private FileInfo CreateDefaultJourneyFileinfo()
        {
            var journeyPath = Path.Combine(appDir.FullName, JourneyDefaultFilename);
            var journeyFileinfo = new FileInfo(journeyPath);
            return journeyFileinfo;
        }

        private void ConfigureKeys(PersistedConfiguration persistedConfiguration, List<string> log, Configuration configuration)
        {
            if (persistedConfiguration.Key == null)
            {
                log.Add("no configuration for keys found");
                return;
            }

            if (persistedConfiguration.Key.Menu != null)
            {
                void Apply(PersistedMenuKeyConfiguration persistedMenuKeyConfiguration,
                    Func<PersistedMenuKeyConfiguration, string> getConfigValue,
                    Configuration.Menu configurationMenu,
                    Action<Configuration.Menu, Key> setKey)
                {
                    var value = getConfigValue(persistedMenuKeyConfiguration);

                    if (!string.IsNullOrEmpty(value))
                    {
                        if (value.Length == 1)
                        {
                            setKey(configurationMenu, (Key)value[0]);
                        }
                        else
                        {
                            if (Dictionary.TryGetValue(value, out var key))
                            {
                                setKey(configurationMenu, key);
                            }
                        }
                    }
                }

                Apply(persistedConfiguration.Key.Menu, x => x.RandomGame, configuration.KeysConfig.Menu, (config, key) => { config.RandomGame = key; });
                Apply(persistedConfiguration.Key.Menu, x => x.ChooseGame, configuration.KeysConfig.Menu, (config, key) => { config.ChooseGame = key; });
                Apply(persistedConfiguration.Key.Menu, x => x.Help, configuration.KeysConfig.Menu, (config, key) => { config.Help = key; });
                Apply(persistedConfiguration.Key.Menu, x => x.Status, configuration.KeysConfig.Menu, (config, key) => { config.Status = key; });
            }
            else
            {
                log.Add("no configuration for menu keys found");
            }

            if (persistedConfiguration.Key.Game != null)
            {
                void Apply(PersistedGameKeyConfiguration persistedGameKeyConfiguration,
                    Func<PersistedGameKeyConfiguration, string> getConfigValue,
                    Configuration.Game configurationGame,
                    Action<Configuration.Game, Key> setKey)
                {
                    var value = getConfigValue(persistedGameKeyConfiguration);

                    if (!string.IsNullOrEmpty(value))
                    {
                        if (value.Length == 1)
                        {
                            setKey(configurationGame, (Key)value[0]);
                        }
                        else
                        {
                            if (Dictionary.TryGetValue(value, out var key))
                            {
                                setKey(configurationGame, key);
                            }
                        }
                    }
                }

                Apply(persistedConfiguration.Key.Game, x => x.Cancel, configuration.KeysConfig.Game, (config, key) => { config.Cancel = key; });

                Apply(persistedConfiguration.Key.Game, x => x.Undo, configuration.KeysConfig.Game, (config, key) => { config.Undo = key; });

                Apply(persistedConfiguration.Key.Game, x => x.Cell0, configuration.KeysConfig.Game, (config, key) => { config.Cell0 = key; });
                Apply(persistedConfiguration.Key.Game, x => x.Cell1, configuration.KeysConfig.Game, (config, key) => { config.Cell1 = key; });
                Apply(persistedConfiguration.Key.Game, x => x.Cell2, configuration.KeysConfig.Game, (config, key) => { config.Cell2 = key; });
                Apply(persistedConfiguration.Key.Game, x => x.Cell3, configuration.KeysConfig.Game, (config, key) => { config.Cell3 = key; });

                Apply(persistedConfiguration.Key.Game, x => x.Column0, configuration.KeysConfig.Game, (config, key) => { config.Column0 = key; });
                Apply(persistedConfiguration.Key.Game, x => x.Column1, configuration.KeysConfig.Game, (config, key) => { config.Column1 = key; });
                Apply(persistedConfiguration.Key.Game, x => x.Column2, configuration.KeysConfig.Game, (config, key) => { config.Column2 = key; });
                Apply(persistedConfiguration.Key.Game, x => x.Column3, configuration.KeysConfig.Game, (config, key) => { config.Column3 = key; });
                Apply(persistedConfiguration.Key.Game, x => x.Column4, configuration.KeysConfig.Game, (config, key) => { config.Column4 = key; });
                Apply(persistedConfiguration.Key.Game, x => x.Column5, configuration.KeysConfig.Game, (config, key) => { config.Column5 = key; });
                Apply(persistedConfiguration.Key.Game, x => x.Column6, configuration.KeysConfig.Game, (config, key) => { config.Column6 = key; });
                Apply(persistedConfiguration.Key.Game, x => x.Column7, configuration.KeysConfig.Game, (config, key) => { config.Column7 = key; });

                Apply(persistedConfiguration.Key.Game, x => x.Foundation0, configuration.KeysConfig.Game, (config, key) => { config.Foundation0 = key; });
                Apply(persistedConfiguration.Key.Game, x => x.Foundation1, configuration.KeysConfig.Game, (config, key) => { config.Foundation1 = key; });
                Apply(persistedConfiguration.Key.Game, x => x.Foundation2, configuration.KeysConfig.Game, (config, key) => { config.Foundation2 = key; });
                Apply(persistedConfiguration.Key.Game, x => x.Foundation3, configuration.KeysConfig.Game, (config, key) => { config.Foundation3 = key; });
            }
            else
            {
                log.Add("no configuration for game keys found");
            }
        }

        private class PersistedConfiguration
        {
            [JsonProperty("JourneyPath")]
            public string JourneyPath { get; private set; }

            [JsonProperty("UiCulture")]
            public string UiCulture { get; private set; }

            [JsonProperty("Key")]
            public PersistedKeyConfiguration Key { get; private set; }
        }

        private class PersistedKeyConfiguration
        {
            [JsonProperty("Menu")]
            public PersistedMenuKeyConfiguration Menu { get; private set; }

            [JsonProperty("Game")]
            public PersistedGameKeyConfiguration Game { get; private set; }
        }

        private class PersistedMenuKeyConfiguration
        {
            [JsonProperty("RandomGame")]
            public string RandomGame { get; private set; }

            [JsonProperty("ChooseGame")]
            public string ChooseGame { get; private set; }

            [JsonProperty("Help")]
            public string Help { get; private set; }

            [JsonProperty("Status")]
            public string Status { get; private set; }
        }

        private class PersistedGameKeyConfiguration
        {
            [JsonProperty("Cancel")]
            public string Cancel { get; private set; }

            [JsonProperty("Undo")]
            public string Undo { get; private set; }

            [JsonProperty("Cell0")]
            public string Cell0 { get; private set; }

            [JsonProperty("Cell1")]
            public string Cell1 { get; private set; }

            [JsonProperty("Cell2")]
            public string Cell2 { get; private set; }

            [JsonProperty("Cell3")]
            public string Cell3 { get; private set; }

            [JsonProperty("Column0")]
            public string Column0 { get; private set; }

            [JsonProperty("Column1")]
            public string Column1 { get; private set; }

            [JsonProperty("Column2")]
            public string Column2 { get; private set; }

            [JsonProperty("Column3")]
            public string Column3 { get; private set; }

            [JsonProperty("Column4")]
            public string Column4 { get; private set; }

            [JsonProperty("Column5")]
            public string Column5 { get; private set; }

            [JsonProperty("Column6")]
            public string Column6 { get; private set; }

            [JsonProperty("Column7")]
            public string Column7 { get; private set; }

            [JsonProperty("Foundation0")]
            public string Foundation0 { get; private set; }

            [JsonProperty("Foundation1")]
            public string Foundation1 { get; private set; }

            [JsonProperty("Foundation2")]
            public string Foundation2 { get; private set; }

            [JsonProperty("Foundation3")]
            public string Foundation3 { get; private set; }
        }
    }
}
