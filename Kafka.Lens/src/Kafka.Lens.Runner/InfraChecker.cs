using Confluent.Kafka;
using Kafka.Lens.Backend;
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
            var topicName = _setting.General.Kafka.TopicName;
            var certificateLocation = _setting.General.Kafka.SslCertificateLocation;
            var certificateSubject = _setting.General.Kafka.SslCertificateSubject;
            var date = DateTime.Now.ToString("dd.MM.yyyy.HH.m");
            topicName = $"{topicName}_{date}";

            if (File.Exists(certificateLocation))
            {
                // delete an old certificate
                File.Delete(certificateLocation);
            }

            var counter = 0;
            foreach (var cluster in clusters)
            {
                counter++;
                var statusReport = new Report();
                statusReport.Number = counter;
                statusReport.EnvName = cluster.Name;
                _logger.Info($" [{counter}] from [{clusters.Count}]. " +
                    $"Work with the '{cluster.Name}' Kafka cluster");
                var bootStrapServers = string.Join(",", cluster.BootstrapServers);
                _logger.Info($" Bootstrap servers: {bootStrapServers}");

                var clientConfig = new ClientConfig
                {
                    BootstrapServers = bootStrapServers,
                    SocketTimeoutMs = connectionTimeoutSec * 1000,
                };
                if (cluster.SslEnabled)
                {
                    _logger.Info("SSL connection is enabled for this cluster");
                    if (!File.Exists(certificateLocation))
                    {
                        var certificate = CertificateHelper.GetCertificate(certificateSubject);
                        CertificateHelper.ExportToPEMFile(certificate, certificateLocation);
                    }
                    clientConfig.SslCaLocation = certificateLocation;
                    clientConfig.SecurityProtocol = SecurityProtocol.Ssl;
                    clientConfig.Debug = "security";
                }
                var bootstrapServersCount = cluster.BootstrapServers.Count;
                var topicHelper = new TopicHelper();
                var topicWasCreated = false;
                var producerConsumer = new ProducerConsumer();

                var connectionIsOk = topicHelper
                    .CheckConnectivity(clientConfig,
                    bootstrapServersCount);

                if (connectionIsOk)
                {
                    topicWasCreated = topicHelper
                         .CreateTopic(clientConfig, bootstrapServersCount, topicName);
                    _logger.Info(string.Empty);
                    producerConsumer
                    .Produce(clientConfig, topicName);

                    producerConsumer
                   .Consume(clientConfig, topicName);
                }

                var mongoDbHelper = new MongoDbHelper();
                var mongoDbConnectionStr = cluster.MongoDb;
                var mongoDBStatus = mongoDbHelper.Ping(mongoDbConnectionStr, connectionTimeoutSec);
                if (mongoDBStatus)
                {
                    statusReport.MongoStatus = "OK";
                }
                else
                {
                    statusReport.MongoStatus = "ERROR";
                }
                if (topicWasCreated)
                {
                    statusReport.KafkaStatus = "OK";
                }
                else
                {
                    statusReport.KafkaStatus = "ERROR";
                }

                if (topicWasCreated && mongoDBStatus)
                {
                    _logger.Info($"The '{cluster.Name}' Kafka cluster status is [OK]");
                }
                else
                {
                    _logger.Error($"The '{cluster.Name}' Kafka cluster status is [ERROR]");
                }
                StatusReportList.Add(statusReport);
                _logger.Info(string.Empty);
            }
            new HtmlReportHelper().PopulateTemplate(StatusReportList);
        }
    }
}
