using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

namespace WebTester
{
    public class Tests
    {
        ChromeDriver _Driver;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions()
            {
                //chromium downloaded from https://www.googleapis.com/download/storage/v1/b/chromium-browser-snapshots/o/Win_x64%2F979993%2Fchrome-win.zip?generation=1646957935320553&alt=media
                //BinaryLocation = @"C:\Users\jason\Downloads\Win_x64_979993_chrome-win\chrome-win\chrome.exe",
            };
            options.AddArgument("headless");
            _Driver = new ChromeDriver(".", options);
        }

        [Test]
        public void Test1()
        {
            _Driver.Url = "https://google.com";
            _Driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
            var searchInput = _Driver.FindElement(By.Name("q"));
            var googleSearchButton = _Driver.FindElement(By.Name("btnK"));

            searchInput.SendKeys("centrolutions\r\n");

            var searchResultsDiv = _Driver.FindElement(By.Id("search"));
            var secondResult = searchResultsDiv.FindElements(By.TagName("a"))[1];
            _Driver.GetScreenshot().SaveAsFile("screenshot.jpg", ScreenshotImageFormat.Jpeg);

            Assert.True(secondResult.Text.Contains("YouTube"));
        }

        [TearDown]
        public void Teardown()
        {
            _Driver.Close();
            _Driver.Quit();
        }
    }
}