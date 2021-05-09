using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reddit;
using Reddit.AuthTokenRetriever;
using StockPulse.Tests;
using System.Diagnostics;

namespace StockPulse.Reddit.Tests
{
    [TestClass]
    public class RedditBaseTests
    {
        [TestMethod]
        public void RedditConnectionTest()
        {
            var tokens = TestHelper.GetCreds();

            var reddit = new RedditClient(
                appId: tokens.AppId,
                appSecret: tokens.AppSecret,
                refreshToken: tokens.RefreshToken);

            var testRes = reddit.Subreddit("wallstreetbets").Posts.GetNew();

            Assert.IsNotNull(testRes);
        }

        /// <summary>
        /// Access + Refresh tokens retrieve method
        /// Uncomment TestMethod line to use
        /// use http://localhost:8080/Reddit.NET/oauthRedirect in redirect_uri
        /// </summary>
        //[TestMethod]
        public void RedditAuthTest()
        {
            var tokens = TestHelper.GetCreds();

            var authTokenRetrieverLib = new AuthTokenRetrieverLib(tokens.AppId, tokens.AppSecret);

            authTokenRetrieverLib.AwaitCallback();

            OpenBrowser(authTokenRetrieverLib.AuthURL());

            while (true) { }

            authTokenRetrieverLib.StopListening();

            Assert.IsNotNull(authTokenRetrieverLib.RefreshToken);

            void OpenBrowser(string authUrl, string browserPath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe")
            {
                try
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(authUrl);
                    Process.Start(processStartInfo);
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    // This typically occurs if the runtime doesn't know where your browser is.  Use BrowserPath for when this happens.  --Kris
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(browserPath)
                    {
                        Arguments = authUrl
                    };
                    Process.Start(processStartInfo);
                }
            }
        }
    }
}
