using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reddit;
using Reddit.Controllers;
using Serilog;
using StockPulse.Database;
using StockPulse.Database.Extensions;
using StockPulse.Reddit.Subreddits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StockPulse.Reddit.Worker
{
    public class PulseWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;
        private readonly RedditClient _redditClient;

        private List<Subreddit> _monitoredSubreddits;

        public PulseWorker(
            IConfiguration config, 
            IServiceProvider serviceProvider,
            RedditClient redditClient)
        {
            _config = config;
            _serviceProvider = serviceProvider;
            _redditClient = redditClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var context = _serviceProvider.GetRequiredService<StockContext>();
            await DatabasePrepareFactory.PrepareDb(context, _config);

            _monitoredSubreddits = await _redditClient
                .GetConfigSubreddits(_config["SubredditListPath"])
                .ToListAsync();

            Log.Information("Tracker started successfully");
        }
    }
}
