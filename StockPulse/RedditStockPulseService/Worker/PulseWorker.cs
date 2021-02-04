using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reddit;
using Serilog;
using StockPulse.Database;
using StockPulse.Database.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StockPulse.Reddit.Worker
{
    public class PulseWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _config;
        private readonly RedditClient _redditClient;

        public PulseWorker(IConfiguration config, IServiceScopeFactory scopeFactory)
        {
            _config = config;
            _scopeFactory = scopeFactory;

            _redditClient = new RedditClient(
                appId: _config["AppId"],
                appSecret: _config["AppSecret"],
                refreshToken: _config["RefreshToken"]);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<StockContext>();

            var nyseTickers = StockListHelper.ReadTickersFromFile(_config["NyseStockList"]);
            await context.AddTickersToDbContext(nyseTickers, "NYSE");

            var nasdaqTickers = StockListHelper.ReadTickersFromFile(_config["NasdaqStockList"]);
            await context.AddTickersToDbContext(nasdaqTickers, "NASDAQ");

            await context.SaveChangesAsync();

            Log.Information("Tracker started successfully");
        }
    }
}
