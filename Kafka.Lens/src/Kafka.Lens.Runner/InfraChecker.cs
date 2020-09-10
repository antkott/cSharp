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

        public List<Report> StatusReportList { get; set; }

        public void Check()
        {
            StatusReportList = new List<Report>();
            var clusters = _setting.General.Kafka.Clusters;
            var connectionTimeoutSec = _setting.General.Kafka.ConnectionTimeoutSec;
            var topicNameFromSettings = _setting.General.Kafka.TopicName;
            var certificateLocation = _setting.General.Kafka.SslCertificateLocation;
            var certificateSubject = _setting.General.Kafka.SslCertificateSubject;
            var date = DateTime.Now.ToString("dd.MM.yyyy.HH.m");


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
                var statusReport = new Report();
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
                var bootStrapServers = string.Join(",", cluster.BootstrapServers);
                _logger.Info($" bootstrap servers: {bootStrapServers}");
                var clientConfig = new ClientConfig
                {
                    BootstrapServers = bootStrapServers,
                    SocketTimeoutMs = connectionTimeoutSec * 1000,
                };
                topicName = $"{topicNameFromSettings}.{date}";
                if (cluster.SslEnabled)
                {
                    _logger.Info("SSL connection is enabled for this cluster");
                    if (!File.Exists(certificateLocation))
                    {
                        try
                        {
                            var certificate = CertificateHelper.GetCertificate(certificateSubject);
                            CertificateHelper.ExportToPEMFile(certificate, certificateLocation);
                        }
                        catch (CertificateException ce)
                        {
                            _logger.Error(ce.Message);
                            kafkaStatus = ReportStatus.CertificateError;
                            _logger.Warn($" Kafka status - [{kafkaStatus}]");
                            WriteClusterStatus(cluster, statusReport, mongoDbStatus, kafkaStatus);
                            StatusReportList.Add(statusReport);
                            continue;
                        }
                    }
                    clientConfig.SslCaLocation = certificateLocation;
                    clientConfig.SecurityProtocol = SecurityProtocol.Ssl;
                    clientConfig.Debug = "security";
                    topicName = $"{topicNameFromSettings}.ssl.{date}";
                }
                var bootstrapServersCount = cluster.BootstrapServers.Count;
                var topicHelper = new TopicHelper();
                var producerConsumer = new ProducerConsumer();

                var connectionIsOk = topicHelper
                    .CheckConnectivity(clientConfig,
                    bootstrapServersCount);

                if (connectionIsOk)
                {
                    var topicWasCreated = topicHelper
                        .CreateTopic(clientConfig, bootstrapServersCount, topicName);
                    if (topicWasCreated)
                    {
                        _logger.Info(string.Empty);
                        var producedMessageCount = producerConsumer.Produce(clientConfig, topicName);
                        var consumedMessageCount = producerConsumer.Consume(clientConfig, topicName);

                        if (producedMessageCount == consumedMessageCount)
                        {
                            _logger.Info($" * Produced messages == consumed messages: '{consumedMessageCount}' - [ok]");
                            kafkaStatus = ReportStatus.Ok;
                        }
                        else
                        {
                            _logger.Error($" * Produced messages != consumed messages: '{consumedMessageCount}' - [error]");
                            kafkaStatus = ReportStatus.Error;
                        }
                    }
                }
                else {
                    kafkaStatus = ReportStatus.Error;
                }
                _logger.Info($" Kafka status - [{kafkaStatus}]");
                WriteClusterStatus(cluster, statusReport, mongoDbStatus, kafkaStatus);
                StatusReportList.Add(statusReport);
                _logger.Info(string.Empty);
            }
            new HtmlReportHelper().PopulateTemplate(StatusReportList);
        }

        private void WriteClusterStatus(
            ClusterSetting cluster,
            Report statusReport,
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
