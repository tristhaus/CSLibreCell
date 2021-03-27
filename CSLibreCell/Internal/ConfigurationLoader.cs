using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security;

namespace CSLibreCell.Internal
{
    /// <summary>
    /// Helper class to load the configuration, providing defaults as necessary.
    /// </summary>
    internal class ConfigurationLoader
    {
        private const string ConfigFilename = "config.json";
        private const string JourneyDefaultFilename = "journey.cslibrecell";
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

                return (new Configuration(journeyFileinfo, uiCulture), log);
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

        private class PersistedConfiguration
        {
            [JsonProperty("JourneyPath")]
            public string JourneyPath { get; private set; }

            [JsonProperty("UiCulture")]
            public string UiCulture { get; private set; }
        }
    }
}
