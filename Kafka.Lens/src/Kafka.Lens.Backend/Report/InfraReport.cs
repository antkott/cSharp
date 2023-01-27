using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka.Lens.Backend.Report
{
    public class InfraReport
    {
        public int Number { get; set; }

        public string EnvName { get; set; }

        public string KafkaStatus { get; set; }

        public string MongoStatus { get; set; }
    }
}
