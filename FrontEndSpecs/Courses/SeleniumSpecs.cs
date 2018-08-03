namespace FrontEnd.Courses
{
    using FluentAssertions;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using Xbehave;

    public class SeleniumSpecs
    {    
        private SeleniumServerFactory<Startup> server;
        private IWebDriver browser;

        [Background]
        public void Background()
        {
            "Start Server".x(() =>
            {
                this.server = new SeleniumServerFactory<Startup>();
                this.server.StartupServerInstance();

                this.browser = new ChromeDriver(
                    @"D:\selenium");
            }).Teardown(() =>
            {
                this.browser.Dispose();
                this.server.Dispose();
            });
        }

        [Scenario]
        public void LoadTheMainPageAndCheckTitle()
        {
            "wenn die Startseite aufgerufen wird".x(()
                => this.browser.Navigate().GoToUrl(this.server.RootUri + "/index.html"));

            "Titel".x(()
                => this.browser.Title.Should().StartWith("Student"));

            "Inhalt".x(()
                => this.browser.FindElement(By.CssSelector("body")).Text.Should().Contain("Hello"));
        }
    }
}