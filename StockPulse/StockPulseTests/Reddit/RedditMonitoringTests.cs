using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reddit.Controllers;
using Reddit.Controllers.EventArgs;
using StockPulse.Reddit.Subreddits;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StockPulse.Tests.Reddit
{
    [TestClass]
    public class RedditMonitoringTests
    {
        [TestMethod]
        public void BaseMonitoringTest()
        {
            var reddit = TestHelper.GetTestRedditClient();

            var newPosts = new List<string>();
            var lockObject = new object();

            var subreddit = reddit.Subreddit("popular");
            subreddit.Posts.GetNew();
            subreddit.Posts.MonitorNew();
            subreddit.Posts.NewUpdated += NewPostsUpdated;

            lock (lockObject)
            {
                Monitor.Wait(lockObject);
            }

            Assert.IsTrue(newPosts.Any());

            void NewPostsUpdated(object sender, PostsUpdateEventArgs e)
            {
                lock (lockObject)
                {
                    newPosts.AddRange(e.Added.Select(p => p.Title));

                    Monitor.PulseAll(lockObject);
                }
            }
        }

        [TestMethod]
        public async Task SubredditsTest()
        {
            var generalSw = new Stopwatch();
            generalSw.Start();

            var reddit = TestHelper.GetTestRedditClient();

            var subreddits = await reddit
                .GetConfigSubreddits(TestHelper.SubredditsListPath)
                .ToArrayAsync();

            Parallel.ForEach(subreddits, subreddit =>
            {
                var sw = new Stopwatch();
                try
                {
                    Log(subreddit, "GetNew started");
                    sw.Start();

                    var posts = subreddit.Posts.GetNew();
                }
                catch (Exception e)
                {
                    Log(subreddit, $"error: {e.Message}");
                }
                finally
                {
                    sw.Stop();
                    Log(subreddit, $"GetNew done elapsed {sw.ElapsedMilliseconds}ms");
                }
            });

            foreach(var subreddit in subreddits)
            {
                subreddit.Posts.MonitorNew();
                subreddit.Posts.NewUpdated += MonitorNewCallback;
            }

            generalSw.Stop();
            Console.WriteLine($"Total {generalSw.ElapsedMilliseconds}ms");

            void Log(Subreddit subreddit, string text) => 
                Console.WriteLine($"(Subreddit: {subreddit.Name}) {text} (general: {generalSw.ElapsedMilliseconds}ms)");

            void MonitorNewCallback(object sender, PostsUpdateEventArgs e) => Console.WriteLine($"added {e.Added.Count} in {e.Added.First().Subreddit}");
        }
    }
}
