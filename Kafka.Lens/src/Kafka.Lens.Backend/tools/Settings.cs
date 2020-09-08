namespace Kafka.Lens.Backend.Tools
{
    
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// by https://app.quicktype.io/?l=csharp
    /// </summary>
    public partial class Settings
    {
        [JsonProperty("General", Required = Required.Always)]
        public General General { get; set; }

        //public override string ToString()
        //{
        //    var general = General.Kafka.Clusters;
        //    foreach (var cluster in general)
        //    {
        //        var bootStr = cluster.BootstrapServers;
        //    }
        //    return "";
        //}
    }

    public partial class General
    {
        [JsonProperty("Kafka", Required = Required.Always)]
        public KafkaSettings Kafka { get; set; }
    }

    public partial class KafkaSettings
    {
        [JsonProperty("Clusters", Required = Required.Always)]
        public List<ClusterSetting> Clusters { get; set; }

        [JsonProperty("TopicName", Required = Required.Always)]
        public string TopicName { get; set; }

        [JsonProperty("PartitionCount", Required = Required.Always)]
        public int PartitionCount { get; set; }

        [JsonProperty("ConnectionTimeoutSec", Required = Required.Always)]
        public int ConnectionTimeoutSec { get; set; }
    }

    public partial class ClusterSetting
    {
        [JsonProperty("Name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("BootstrapServers", Required = Required.Always)]
        public List<string> BootstrapServers { get; set; }
        public string MongoDb { get; set; }
    }
}