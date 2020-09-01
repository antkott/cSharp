using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Kafka.Lens.Backend.Tools;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kafka.Lens.Backend
{
    public class TopicHelper
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
                return false;
            }
            return true;
        }
    }
}

