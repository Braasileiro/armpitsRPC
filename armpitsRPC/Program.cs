using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ArmpitsRPC
{
    public class Program
    {
        static async Task Main()
        {
            Logger.Info($"{BuildInfo.Name} {BuildInfo.Version} by {BuildInfo.Copyright}.");

            await new HostBuilder()
                .UseContentRoot(Environment.CurrentDirectory)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<ProgramService>();
                })
                .UseConsoleLifetime()
                .Build()
                .RunAsync();
        }
    }
}
