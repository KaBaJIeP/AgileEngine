using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AE.ImageGallery.Supplier.Runner
{
    static class Program
    {
        static Task Main(string[] args) => CreateHostBuilder(args).Build().RunAsync();

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("appsettings.json", optional: false );
                    configHost.AddEnvironmentVariables(prefix: "IMAGE_GALLERY_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.ConfigureRunner(hostContext.Configuration);
                    services.AddHostedService<RunnerService>();
                });
    }
}