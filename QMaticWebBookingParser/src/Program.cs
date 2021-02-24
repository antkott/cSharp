using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using QMaticWebBookingParser;

var configuration = new ConfigurationBuilder()
       .AddEnvironmentVariables()
       .AddCommandLine(args)
       .AddJsonFile("appsettings.json")
       .Build();

using IHost host = Host
    .CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(builder =>
    {
        builder.Sources.Clear();
        builder.AddConfiguration(configuration);
    })
    .ConfigureServices((context, services) =>
    {
        IConfiguration config = context.Configuration;
        Prague pragueSettings = config.GetSection("Prague").Get<Prague>();
        Brno brnoSettings = config.GetSection("Brno").Get<Brno>();
        ParserSettings settings = config.Get<ParserSettings>();
        settings.CityPrague = pragueSettings;
        settings.CityBrno = brnoSettings;
        services.AddSingleton(settings);
        services.AddHostedService<ParserWorker>();
    })
    .ConfigureLogging((context, builder) =>
    {
        builder.ClearProviders();
        builder.AddNLog(context.Configuration);
    })
    .Build();
Console.Title = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}";
AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

try
{
    await host.RunAsync();
}
finally
{
    Parser.KillChromeDrivers();
}


async void CurrentDomain_ProcessExit(object sender, EventArgs e)
{
    var source = new CancellationTokenSource();
    source.CancelAfter(TimeSpan.FromSeconds(30));
    Console.WriteLine("exiting...");
    await host?.StopAsync(source.Token);
    Parser.KillChromeDrivers();
}
