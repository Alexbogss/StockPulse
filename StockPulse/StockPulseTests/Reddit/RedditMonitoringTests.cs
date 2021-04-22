using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reddit;
using Reddit.Controllers.EventArgs;
using System.Collections.Generic;
using System.Linq;

namespace StockPulse.Tests.Reddit
{
    [TestClass]
    public class RedditMonitoringTests
    {
        public List<string> NewPosts = new List<string>();

        [TestMethod]
        public void BaseMonitoringTest()
        {
            var tokens = RedditTestHelper.GetCreds();

            var reddit = new RedditClient(
                appId: tokens.AppId,
                appSecret: tokens.AppSecret,
                refreshToken: tokens.RefreshToken);

            var subreddit = reddit.Subreddit("wallstreetbets");
            subreddit.Posts.GetNew();
            subreddit.Posts.MonitorNew();
            subreddit.Posts.NewUpdated += NewPostsUpdated;

            while(true)
            {
                if (NewPosts.Any()) break;
            }

            Assert.IsTrue(NewPosts.Any());
        }

        private void NewPostsUpdated(object sender, PostsUpdateEventArgs e)
        {
            NewPosts.AddRange(e.Added.Select(p => p.Title));
        }
    }
}
