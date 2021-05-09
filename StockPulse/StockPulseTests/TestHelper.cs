using Newtonsoft.Json;
using Reddit;
using System.IO;

namespace StockPulse.Tests
{
    public static class TestHelper
    {
        public const string CommonDataPath = "../../../../Common/Data";

        public static string GetCommonDataFilePath(string filename) => Path.Combine(CommonDataPath, filename);

        public static ClientTokens GetCreds()
        {
            var json = File.ReadAllText(Path.Combine(CommonDataPath, "creds.json"));

            return JsonConvert.DeserializeObject<ClientTokens>(json);
        }

        public struct ClientTokens
        {
            public string AppId;
            public string AppSecret;
            public string RefreshToken;
            public string AccessToken;
        }

        public static RedditClient GetTestRedditClient()
        {
            var tokens = GetCreds();

            return new RedditClient(
                appId: tokens.AppId,
                appSecret: tokens.AppSecret,
                refreshToken: tokens.RefreshToken);
        }

        public static string NasdaqStockListPath => GetCommonDataFilePath("NASDAQ_v1.txt");

        public static string NyseStockListPath => GetCommonDataFilePath("NYSE_v1.txt");

        public static string SubredditsListPath => GetCommonDataFilePath("subreddits.txt");
    }
}
