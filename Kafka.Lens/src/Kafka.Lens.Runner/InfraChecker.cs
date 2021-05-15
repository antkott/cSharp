using Confluent.Kafka;
using Kafka.Lens.Backend;
using Kafka.Lens.Backend.Report;
using Kafka.Lens.Backend.Tools;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kafka.Lens.Runner
{
    public class InfraChecker
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Settings _setting = new SettingsReader().Settings;
        private readonly KafkaHelper _kafkaHelper = new KafkaHelper();

        public List<InfraReport> StatusReportList { get; set; }

        public void Check()
        {
            StatusReportList = new List<InfraReport>();
            var clusters = _setting.General.Kafka.Clusters;
            var connectionTimeoutSec = _setting.General.Kafka.ConnectionTimeoutSec;
            var topicNameFromSettings = _setting.General.Kafka.TopicName;
            var certificateLocation = _setting.General.Kafka.SslCertificateLocation;
            var certificateSubject = _setting.General.Kafka.SslCertificateSubject;
            


            if (File.Exists(certificateLocation))
            {
                // delete an old certificate
                File.Delete(certificateLocation);
            }

            var counter = 0;
            foreach (var cluster in clusters)
            {
                counter++;
                string topicName;
                var kafkaStatus = ReportStatus.Undefined;
                var mongoDbStatus = ReportStatus.Undefined;
                var statusReport = new InfraReport();
                statusReport.Number = counter;
                statusReport.EnvName = cluster.Name;
                _logger.Info($" [{counter}] from [{clusters.Count}]. " +
                    $"Work with the '{cluster.Name}' cluster");

                // Check Mongo
                _logger.Info($"Checking Mongo DB:");
                var mongoDbHelper = new MongoDbHelper();
                var mongoDbConnectionStr = cluster.MongoDb;
                mongoDbStatus = mongoDbHelper.Ping(mongoDbConnectionStr, connectionTimeoutSec);

                // Check Kafka
                _logger.Info("Checking Kafka:");
                var kafkaHelper = new KafkaHelper();
                kafkaStatus = kafkaHelper.CheckStatus(_setting, cluster);

                WriteClusterStatus(cluster, statusReport, mongoDbStatus, kafkaStatus);
                StatusReportList.Add(statusReport);
                _logger.Info(string.Empty);
            }
            new HtmlReportHelper().PopulateTemplate(StatusReportList);
        }

        private void WriteClusterStatus(
            ClusterSetting cluster,
            InfraReport statusReport,
            string mongoStatus,
            string kafkaStatus)
        {

            statusReport.MongoStatus = mongoStatus;
            statusReport.KafkaStatus = kafkaStatus;

            if (kafkaStatus.Equals(ReportStatus.Ok) && mongoStatus.Equals(ReportStatus.Ok))
            {
                _logger.Info($"The '{cluster.Name}' status is [{ReportStatus.Ok.ToUpper()}]");
            }
            else
            {
                _logger.Error($"The '{cluster.Name}' status is [{ReportStatus.Error.ToUpper()}]");
            }
        }
    }
}
