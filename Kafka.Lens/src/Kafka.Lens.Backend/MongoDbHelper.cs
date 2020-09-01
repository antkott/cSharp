using MongoDB.Driver;
using NLog;
using System;
using System.Collections.Generic;

namespace Kafka.Lens.Backend
{
    public class MongoDbHelper
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public bool Ping(string connectionString, int timeoutSec)
        {

            _logger.Info($"Checking Mongo DB:");
            var connectionArray = connectionString.Split(":");
            var dbClient = new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(connectionArray[0], int.Parse(connectionArray[1])),
                
                SocketTimeout = TimeSpan.FromSeconds(timeoutSec),
                WaitQueueTimeout = TimeSpan.FromSeconds(timeoutSec),
                ConnectTimeout = TimeSpan.FromSeconds(timeoutSec),
                ServerSelectionTimeout = TimeSpan.FromSeconds(timeoutSec)
            });
            try
            {
                var dbList = dbClient.ListDatabases().ToList();
                _logger.Info($"received '{dbList.Count}' DBs");
                if (dbList.Count > 0)
                {
                    _logger.Info($" * The '{connectionString}' Mongo DB status - [ok]");
                    return true;
                }
                else
                {
                    _logger.Error($" * The '{connectionString}' Mongo DB status - [error]");
                    return false;
                }
            }
            catch (Exception e) {
                // _logger.Error(e.Message);
                _logger.Error($" * The '{connectionString}' Mongo DB status - [error]");
                return false;
            }
            
        }
    }
}
