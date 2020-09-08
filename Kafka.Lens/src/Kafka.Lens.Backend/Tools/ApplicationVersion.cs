using Microsoft.Extensions.Logging;
using NLog;
using System;

namespace Kafka.Lens.Backend.Tools
{
    public static class ApplicationVersion
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static string Version = "0.1";
        public static string Autor = "Anton V. Kotliarenko";
        public static string ApplicationName = "Kafka Lens";
        private static string VersionStr1 = $"{ApplicationName} [Version {Version}]";
        private static string VersionStr2 = $"(c) 2020 {Autor}. All rights reserved.";

        public static void PrintConsoleAbout()
        {
            
            Console.WriteLine(VersionStr1);
            Console.WriteLine(VersionStr2);
            Console.WriteLine();
        }

        public static void PrintAbout()
        {
            _logger.Info(VersionStr1);
            _logger.Info(VersionStr2);
            _logger.Info(string.Empty);
        }

    }
}
