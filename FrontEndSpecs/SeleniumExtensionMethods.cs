namespace FrontEnd
{
    using System;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public static class SeleniumExtensionMethods
    {
        public static void Open<T>(
            this IWebDriver driver,
            SeleniumServerFactory<T> server,
            string urlPart)
            where T : class
        {
            var url = CombineUrl(
                server.RootUri,
                urlPart);

            driver.Navigate().GoToUrl(
                url);
        }

        private static string CombineUrl(
            string baseUrl,
            string relativeUrl) {
            var baseUri = new UriBuilder(baseUrl);

            if (Uri.TryCreate(baseUri.Uri, relativeUrl, out var newUri))
                return newUri.ToString();

            throw new ArgumentException("Unable to combine specified url values");
        }

        public static async Task For(
            this WebDriverWait wait,
            Action test)
        {
            var start = DateTime.UtcNow;
            var maxEnd = start + wait.Timeout;
            Exception lastException;
            do
            {
                try
                {
                    test();
                    return;
                }
                catch (Exception e)
                {
                    lastException = e;
                    await Task.Delay(wait.PollingInterval);
                }
            } while(DateTime.UtcNow < maxEnd);

            if (lastException != null)
            {
                throw lastException;
            }
        }
    }
}