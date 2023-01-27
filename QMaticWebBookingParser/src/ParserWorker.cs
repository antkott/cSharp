using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using System.Media;

namespace QMaticWebBookingParser
{
    public sealed class ParserWorker : BackgroundService
    {
        private readonly ILogger<ParserWorker> _logger;
        private readonly NLog.ILogger _specialLogger = LogManager.GetLogger("SpecialLogger");
        private readonly ParserSettings _parserSettings;
        private readonly Parser _parser;
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
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    counter++;

                    var timeslots = _parser.Parse(stoppingToken)
                        .Result;
                    var line = $"{counter} attempt,";
                    if (timeslots.AreFreePlaces(_parserSettings.NoPlacesMessage, _logger, out var result))
                    {
                        line = $"{line} ALARM find, {result}";
                        _logger.LogError(line);
                        Console.Title = $"{counter} ALAAAAARRRRRMMMM!!!";
                        _specialLogger.Warn(line);
                        for (var i = 0; i < 10; i++)
                        {
                            SystemSounds.Exclamation.Play();
                            await Task.Delay(1000, stoppingToken);
                        }
                    }
                    else
                    {
                        line = $"{line} {result}";
                        _logger.LogInformation(line);
                        Console.Title = $"{result} {counter}";
                        _specialLogger.Info("No places");
                    }
                    await Task.WhenAny(Task.Delay(_parserSettings.AttemptsDelaySec * 1000, stoppingToken)); // Ignores the TaskCanceledException & exception
                    if (counter == 1)
                    {
                        Console.Clear();
                    }

                }
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
                _logger.LogError($"exp: {ex}");
                _specialLogger.Info("error");
                await Task.Delay(1_000, stoppingToken);
                _logger.LogInformation($"restarting...");
            }
            finally
            {
                _parser?.Dispose();
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _stop = true;
            _logger.LogInformation($"stopping async");
            _parser?.Dispose();
            await base.StopAsync(cancellationToken);

        }
    }
}
