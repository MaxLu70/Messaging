using ImuConsumer.Consumer.Imu;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = new HostBuilder().ConfigureServices((hostContext, services) =>
{
    services.AddHostedService<ElencoContribuentiConsumer>();
});

await builder.RunConsoleAsync();