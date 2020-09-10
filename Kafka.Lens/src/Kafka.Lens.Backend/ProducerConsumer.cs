using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Kafka.Lens.Backend.Tools;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka.Lens.Backend
{
    public class ProducerConsumer
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _key = "kafka.lens.test.key";
        private readonly int _numMessages = 3;

        public int Produce(ClientConfig config, string topic)
        {
            _logger.Info($"  Produce to the '{topic}' topic: ");
            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                int numProduced = 0;
                for (int i = 0; i < _numMessages; ++i)
                {
                    var val = JObject.FromObject(new { count = i }).ToString(Formatting.None);
                    //_logger.Info($"Producing record: {_key} {val}");
                    producer.Produce(topic, new Message<string, string> { Key = _key, Value = val },
                        (deliveryReport) =>
                        {
                            if (deliveryReport.Error.Code != ErrorCode.NoError)
                            {
                                _logger.Error($"Failed to deliver message: {deliveryReport.Error.Reason}");
                            }
                            else
                            {
                                _logger.Info($"produced message to: {deliveryReport.TopicPartitionOffset}");
                                numProduced += 1;
                            }
                        });
                }
                producer.Flush(TimeSpan.FromSeconds(10));

                _logger.Info($"{numProduced} messages were produced to topic {topic}");
                return numProduced;
            }
        }


        public int Consume(ClientConfig config, string topic)
        {
            _logger.Info($"  Read from the '{topic}' topic:");
            var consumerConfig = new ConsumerConfig(config)
            {
                GroupId = "kafka.lens-group-1",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            //CancellationTokenSource cts = new CancellationTokenSource();
            //Console.CancelKeyPress += (_, e) =>
            //{
            //    e.Cancel = true; // prevent the process from terminating.
            //    cts.Cancel();
            //};

            using (var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build())
            {
                consumer.Subscribe(topic);
                var totalCount = 0;
                try
                {
                    while (totalCount < _numMessages)
                    {
                        var cr = consumer.Consume();
                        //var cr = consumer.Consume(cts.Token);
                        totalCount += JObject.Parse(cr.Message.Value).Value<int>("count");
                        _logger.Info($"consumed message with key {cr.Message.Key} and value {cr.Message.Value}");
                    }
                }//catch (OperationCanceledException)
                //{
                //    // Ctrl-C was pressed.
                //}

                //catch (OperationCanceledException)
                //{
                //    // Ctrl-C was pressed.
                //}
                finally
                {
                    consumer.Close();
                }                
                return totalCount;
            }
        }
    }
}
