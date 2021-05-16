using Confluent.Kafka;
using Kafka.Lens.Backend;
using Kafka.Lens.Backend.Report;
using Kafka.Lens.Backend.Tools;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

            var mongoDbHelper = new MongoDbHelper();
            var kafkaHelper = new KafkaHelper();
            var counter = 0;
            var tasksList = new List<Task<StatusCheckResult>>();
            foreach (var cluster in clusters)
            {
                counter++;
                string topicName;
                var kafkaStatus = ReportStatus.Undefined;
                var mongoDbStatus = ReportStatus.Undefined;
                var statusReport = new InfraReport();
                statusReport.Number = counter;
                statusReport.EnvName = cluster.Name;
                var initialMessage = $"Work with the '{cluster.Name}' cluster";
                _logger.Info($" Add status check task -  [{counter}] from [{clusters.Count}]. " +
                    $" {initialMessage}");

                // Check Mongo
                var taskMongo = new Task<StatusCheckResult>(() => mongoDbHelper.Ping(cluster.MongoDb, connectionTimeoutSec, initialMessage));
                tasksList.Add(taskMongo);
                taskMongo.Start();

                // Check Kafka
                //_logger.Info("Checking Kafka:");
                //var taskKafka = new Task<string>(() => kafkaHelper.CheckStatus(_setting, cluster));
                //tasksList.Add(taskKafka);
                //taskKafka.Start();
                //WriteClusterStatus(cluster, statusReport, mongoDbStatus, kafkaStatus);
                //StatusReportList.Add(statusReport);
                //_logger.Info(string.Empty);
            }
            Task.WaitAll(tasksList.ToArray());
            _logger.Info(string.Empty);
            _logger.Info("REPORT");
            foreach (var statusCheckResult in tasksList)
            {
                if (statusCheckResult.Result.InfraType == InfraType.Mongo) {
                    _logger.Info(statusCheckResult.Result.LogOutput);
                }
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
                _logger.Info($"The '{cluster.Name}' status is [{ReportStatus.Ok}]");
            }
            else
            {
                _logger.Error($"The '{cluster.Name}' status is [{ReportStatus.Error}]");
            }
        }
    }
}
