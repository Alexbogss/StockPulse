using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using StockPulse.Database;
using StockPulse.Reddit.Worker;
using System.IO;

namespace StockPulse.Reddit
{
    public class Program
    {
        public static void Main()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Verbose()
                .CreateLogger();

            CreateHostBuilder(config).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(IConfiguration config) =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(_ => config);
                    services.AddHostedService<PulseWorker>();

                    services.AddDbContext<StockContext>(options =>
                        options.UseSqlite(config["DbConnectionString"]), ServiceLifetime.Transient);
                });
    }
}
