using Newtonsoft.Json;
using System;
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
        internal Configuration Read()
        {
            var configFilepath = Path.Combine(appDir.FullName, ConfigFilename);
            var configFileinfo = new FileInfo(configFilepath);

            if (!configFileinfo.Exists)
            {
                return this.CreateDefaultConfiguration();
            }

            var content = File.ReadAllText(configFileinfo.FullName);

            if (string.IsNullOrWhiteSpace(content))
            {
                return this.CreateDefaultConfiguration();
            }

            try
            {
                var persistedConfiguration = JsonConvert.DeserializeObject<PersistedConfiguration>(content);

                if (!IsValidPath(persistedConfiguration.JourneyPath))
                {
                    return this.CreateDefaultConfiguration();
                }

                return new Configuration(new FileInfo(persistedConfiguration.JourneyPath));
            }
            catch (JsonException)
            {
                return this.CreateDefaultConfiguration();
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
            var journeyPath = Path.Combine(appDir.FullName, JourneyDefaultFilename);
            var journeyFileinfo = new FileInfo(journeyPath);

            return new Configuration(journeyFileinfo);
        }

        private class PersistedConfiguration
        {
            [JsonProperty("JourneyPath")]
            public string JourneyPath { get; private set; }
        }
    }
}
