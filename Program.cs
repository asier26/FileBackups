using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace workerservice
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder().Build().Run();
        }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
            .UseWindowsService()
            .ConfigureServices((ctx, services) => {
                services.AddHostedService<Worker>();
            });
        }
    }
}
