using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using QMaticWebBookingParser.Helpers;

namespace QMaticWebBookingParser
{
    public sealed class ParserWorker : BackgroundService
    {
        private readonly ILogger<ParserWorker> _logger;
        private readonly NLog.ILogger _specialLogger = LogManager.GetLogger("SpecialLogger");
        private readonly NLog.ILogger _findLogger = LogManager.GetLogger("FindLogger");
        private readonly ParserSettings _parserSettings;
        private Parser _parser;
        private volatile bool _stop;

        public ParserWorker(
            ILogger<ParserWorker> logger,
            ParserSettings parserSettings)
        {
            _logger = logger;
            _parserSettings = parserSettings;
            _parser = new Parser(_parserSettings);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var counter = 0;
            var city = _parserSettings.City;
            Console.Title = city;
            var noPlacesMessages = _parserSettings.CityPrague.NoPlacesMessage;
            if (city.Equals("Brno"))
            {
                noPlacesMessages = _parserSettings.CityBrno.NoPlacesMessage;
            }
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    counter++;
                    try
                    {
                        if (_parser == null)
                        {
                            _parser = new Parser(_parserSettings);
                        }
                        Console.Title = $"{city} checking..";
                        Dictionary<string, string> timeslots = _parser.Parse(stoppingToken)
                        .Result;

                        var line = $"{counter} attempt,";
                        if (timeslots.AreFreePlaces(noPlacesMessages, out var result))
                        {
                            SoundHelper.PlayFindAlarm();
                            line = $"{city}: {line} (!) ALARM FIND: {result}";
                            Console.Title = $"(!) |{city}| FIND!";
                            _logger.LogWarning(line);
                            _specialLogger.Warn(line);
                            _findLogger.Warn(line);
                        }
                        else
                        {
                            line = $"{line} {result}";
                            _logger.LogInformation(line);
                            Console.Title = $"{city}| No places";
                            _specialLogger.Info($"{city}; No places");
                        }
                        await Task.WhenAny(Task.Delay(_parserSettings.AttemptsDelaySec * 1000, stoppingToken)); // Ignores the TaskCanceledException & exception                        
                    }
                    catch (TaskCanceledException)
                    {
                        _logger.LogInformation($"task cancelled");
                        _parser?.Dispose();
                    }
                    catch (Exception ex)
                    {
                        if (_parser == null || _stop)
                        {
                            return;
                        }
                        _logger.LogError($"{ex.Message}");
                        _specialLogger.Info("error");
                        _logger.LogInformation($"wait before restarting");
                        await Task.Delay(1_000, stoppingToken);
                        _logger.LogInformation($"restarting...");

                    }
                }
            }
            finally
            {
                _parser?.Dispose();
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _stop = true;
            _logger.LogInformation("stopping async");
            _parser?.Dispose();
            await base.StopAsync(cancellationToken);
        }
    }
}
