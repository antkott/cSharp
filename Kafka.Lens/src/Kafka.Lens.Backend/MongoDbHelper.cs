using Kafka.Lens.Backend.Report;
using Kafka.Lens.Backend.Tools;
using MongoDB.Driver;
using System;

namespace Kafka.Lens.Backend
{
    public class MongoDbHelper
    {
        public StatusCheckResult Ping(string connectionString, int timeoutSec, string initialLogMessage)
        {
            var logStashHelper = new LogHelper(initialLogMessage);
            logStashHelper.Info("Checking Mongo DB:");
            var statusCheckResult = new StatusCheckResult
            {
                InfraType = InfraType.Mongo,
                Status = ReportStatus.Undefined
            };
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
                logStashHelper.Info($"received '{dbList.Count}' DBs");
                if (dbList.Count > 0)
                {
                    logStashHelper.Info($" * The '{connectionString}' Mongo DB status - [{ReportStatus.Ok}]");
                    statusCheckResult.Status = ReportStatus.Ok;
                }
                else
                {
                    logStashHelper.Error($" * The '{connectionString}' Mongo DB status - [{ReportStatus.Error}]");
                    statusCheckResult.Status = ReportStatus.Error;
                }
            }
            catch (Exception e)
            {
                // logStashHelper.Error(e.Message);
                logStashHelper.Error($" * The '{connectionString}' Mongo DB status - [{ReportStatus.Error}]");
                statusCheckResult.Status = ReportStatus.Error;
            }
            statusCheckResult.LogOutput = logStashHelper.LogStash;
            return statusCheckResult;
        }
    }
}
