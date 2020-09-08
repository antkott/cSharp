using Confluent.Kafka;
using Kafka.Lens.Backend;
using Kafka.Lens.Backend.Tools;
using NLog;
using System;

namespace Kafka.Lens.Runner
{
    public class InfraChecker
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Settings _setting = new SettingsReader().Settings;

        public void Check()
        {
            var clusters = _setting.General.Kafka.Clusters;
            var connectionTimeoutSec = _setting.General.Kafka.ConnectionTimeoutSec;
            var topicName = _setting.General.Kafka.TopicName;
            var date = DateTime.Now.ToString("dd.MM.yyyy.HH.m");
            topicName = $"{topicName}_{date}";

            var counter = 0;
            foreach (var cluster in clusters)
            {
                counter++;
                _logger.Info($" [{counter}] from [{clusters.Count}]. " +
                    $"Work with the '{cluster.Name}' Kafka cluster");
                var bootStrapServers = string.Join(",", cluster.BootstrapServers);
                _logger.Info($" Bootstrap servers: {bootStrapServers}");

                var clientConfig = new ClientConfig
                {
                    BootstrapServers = bootStrapServers,
                    SocketTimeoutMs = connectionTimeoutSec * 1000,
                };
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

                if (topicWasCreated && mongoDBStatus)
                {
                    _logger.Info($"The '{cluster.Name}' Kafka cluster status is [OK]");
                }
                else
                {
                    _logger.Error($"The '{cluster.Name}' Kafka cluster status is [ERROR]");
                }
                _logger.Info(string.Empty);
            }
        }
    }
}
