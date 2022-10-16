using PuppeteerSharp;

namespace WebTester
{
    public class Tests
    {
        BrowserFetcher _Fetcher;
        IBrowser _Browser;

        [SetUp]
        public async Task Setup()
        {
            _Fetcher = new BrowserFetcher();
            await _Fetcher.DownloadAsync();
            var options = new LaunchOptions()
            {
                Headless = true,
                DefaultViewport = new ViewPortOptions() { Width = 990, Height = 809 }
            };
            _Browser = await Puppeteer.LaunchAsync(options);
        }

        [Test]
        public async Task Test1()
        {
            var networkIdle = new WaitUntilNavigation[] { WaitUntilNavigation.Networkidle0 };

            using var page = await _Browser.NewPageAsync();
            await page.GoToAsync("http://www.google.com", waitUntil: networkIdle);
            var searchInput = await page.QuerySelectorAsync("input[name=q]");
            await searchInput.ClickAsync();
            await searchInput.TypeAsync("centrolutions");
            await page.Keyboard.DownAsync("Enter");
            await page.WaitForNavigationAsync(new NavigationOptions()
            {
                WaitUntil = networkIdle
            });

            var secondResult = await page.QuerySelectorAsync("#rso > div:nth-child(2)");
            var secondResultText = await secondResult.EvaluateFunctionAsync<string>("e => e.innerText");
            await page.ScreenshotAsync("screenshot.png");

            Assert.True(secondResultText.Contains("YouTube"));
        }
    }
}