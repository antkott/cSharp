using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Kafka.Lens.Backend.Report;
using Kafka.Lens.Backend.Tools;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Kafka.Lens.Backend
{
    public class KafkaHelper
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public bool CheckConnectivity(
            ClientConfig clientSettings,
            int bootstrapServersCount)
        {
            var connectionIsOk = true;
            _logger.Info("Checking connectivity");
            try
            {
                if (clientSettings == null)
                {
                    _logger.Error($"cluster settings == null");
                    return false;
                }

                using var adminClient = new AdminClientBuilder(clientSettings).Build();
                var timeout = clientSettings.SocketTimeoutMs / 1000;
                var meta = adminClient.GetMetadata(TimeSpan.FromSeconds((double)timeout));
                _logger.Info($" OriginatingBrokerId: {meta.OriginatingBrokerId} " +
                    $"OriginatingBrokerName: {meta.OriginatingBrokerName}");
                _logger.Info($"brokers:");
                meta.Brokers.ForEach(broker =>
                     _logger.Info($"  {broker.BrokerId} {broker.Host}:{broker.Port}"));
                if (bootstrapServersCount != meta.Brokers.Count)
                {
                    _logger.Error($" * {meta.Brokers.Count} from {bootstrapServersCount} brokers online - [error]");
                    connectionIsOk = false;
                }
                else
                {
                    _logger.Info($" * {meta.Brokers.Count} from {bootstrapServersCount} brokers online - [ok]");
                }
                _logger.Info($"topics count: '{meta.Topics.Count}'");
                //meta.Topics.ForEach(topic =>
                // _logger.Info($"  {topic.Topic}"));
            }
            catch (KafkaException e)
            {
                _logger.Error($"Kafka connectivity error: '{e.Message}'");
                connectionIsOk = false;
            }
            return connectionIsOk;
        }

        public bool CreateTopic(
            ClientConfig clusterSettings,
            int bootstrapServersCount,
            string topicName)
        {
            _logger.Info($"Create topic '{topicName}'");
            using var adminClient = new AdminClientBuilder(clusterSettings).Build();

            try
            {
                adminClient
                    .CreateTopicsAsync(
                    new List<TopicSpecification> {
                        new TopicSpecification {
                            Name = topicName,
                            NumPartitions = bootstrapServersCount*2,
                            ReplicationFactor = (short)bootstrapServersCount
                        } })
                    .Wait();
            }
            catch (CreateTopicsException e)
            {
                if (e.Results[0].Error.Code != ErrorCode.TopicAlreadyExists)
                {
                    _logger.Error($"An error occured creating topic {topicName}: {e.Results[0].Error.Reason}");
                }
                else
                {
                    _logger.Warn("Topic already exists");
                }
                return false;
            }
            catch (AggregateException e)
            {
                if (e.InnerException
                    .Message
                    .StartsWith("An error occurred creating topics:"))
                {
                    _logger.Warn("Topic already exists");
                    return true;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        public ReportStatus CheckStatus(Settings setting, ClusterSetting cluster)
        {
            var connectionTimeoutSec = setting.General.Kafka.ConnectionTimeoutSec;
            var topicNameFromSettings = setting.General.Kafka.TopicName;
            var certificateLocation = setting.General.Kafka.SslCertificateLocation;
            var certificateSubject = setting.General.Kafka.SslCertificateSubject;
            string topicName;
            var date = DateTime.Now.ToString("dd.MM.yyyy.HH.m");
            var kafkaStatus = ReportStatus.Undefined;

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
                        // WriteClusterStatus(cluster, statusReport, mongoDbStatus, kafkaStatus);
                        // StatusReportList.Add(statusReport);
                    }
                }
                clientConfig.SslCaLocation = certificateLocation;
                clientConfig.SecurityProtocol = SecurityProtocol.Ssl;
                clientConfig.Debug = "security";
                topicName = $"{topicNameFromSettings}.ssl.{date}";
            }
            var bootstrapServersCount = cluster.BootstrapServers.Count;
            var producerConsumer = new ProducerConsumer();

            var connectionIsOk = CheckConnectivity(clientConfig,
                bootstrapServersCount);

            if (connectionIsOk)
            {
                var topicWasCreated = CreateTopic(clientConfig, bootstrapServersCount, topicName);
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
            else
            {
                kafkaStatus = ReportStatus.Error;
            }
            _logger.Info($" Kafka status - [{kafkaStatus}]");
            return kafkaStatus;
        }
    }
}

