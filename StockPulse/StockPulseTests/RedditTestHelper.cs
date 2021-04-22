using Newtonsoft.Json;
using System.IO;

namespace StockPulse.Tests.Reddit
{
    public static class RedditTestHelper
    {
        public static ClientTokens GetCreds()
        {
            var json = File.ReadAllText("../../../creds.json");

            return JsonConvert.DeserializeObject<ClientTokens>(json);
        }

        public struct ClientTokens
        {
            public string AppId;
            public string AppSecret;
            public string RefreshToken;
            public string AccessToken;
        }

        public static string GetNasdaqStockListPath()
        {
            return "../../../NASDAQ_v1.txt";
        }

        public static string GetNyseStockListPath()
        {
            return "../../../NYSE_v1.txt";
        }
    }
}
