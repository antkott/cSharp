using Newtonsoft.Json;
using System.IO;

namespace Kafka.Lens.Backend.Tools
{
    public class SettingsReader
    {
        public Settings Settings { get; private set; }

        public SettingsReader()
        {
            Settings = ReadSetting();
        }

        private Settings ReadSetting()
        {
            var json = File.ReadAllText(@"appSettings.json");
            var response = JsonConvert.DeserializeObject<Settings>(json);
            return response;
        }
    }
}


