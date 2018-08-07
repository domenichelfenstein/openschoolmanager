namespace FrontEnd
{
    using System;
    using Backend;
    using Fakes;
    using Microsoft.Extensions.DependencyInjection;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Support.UI;
    using Xbehave;

    public abstract class SeleniumSpec
    {
        protected SeleniumServerFactory<Startup> server;
        protected IWebDriver browser;
        protected WebDriverWait wait;
        protected FakeBackEndFacade backend;

        [Background]
        public void Background()
        {
            "Start Server".x(() =>
            {
                this.backend = new FakeBackEndFacade();

                this.server = new SeleniumServerFactory<Startup>(
                    s => s
                        .AddSingleton<IGuidGenerator>(p => new FakeGuidGenerator())
                        .AddSingleton<IBackEndFacade>(p => this.backend));
                this.server.StartupServerInstance();

                this.browser = new ChromeDriver(
                    @"D:\selenium");

                this.wait = new WebDriverWait(
                    this.browser,
                    TimeSpan.FromSeconds(60));
                this.wait.PollingInterval = TimeSpan.FromMilliseconds(100);
            }).Teardown(() =>
            {
                this.browser.Dispose();
                this.server.Dispose();
            });
        }
    }
}