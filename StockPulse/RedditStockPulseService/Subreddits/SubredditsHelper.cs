using Reddit;
using Reddit.Controllers;
using System.Collections.Generic;
using System.IO;

namespace StockPulse.Reddit.Subreddits
{
    public static class SubredditsHelper
    {
        public static async IAsyncEnumerable<Subreddit> GetConfigSubreddits(this RedditClient reddit, string filepath)
        {
            await foreach(var subredditName in ReadSubredditsFromFile(filepath))
            {
                yield return reddit.Subreddit(subredditName);
            }
        }

        public static async IAsyncEnumerable<string> ReadSubredditsFromFile(string filePath)
        {
            using var reader = new StreamReader(filePath);

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                yield return line.Trim();
            }
        }
    }
}
