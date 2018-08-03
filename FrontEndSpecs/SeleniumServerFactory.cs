namespace FrontEnd
{
    using System.Linq;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Server.Features;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;

    public class SeleniumServerFactory<T> : WebApplicationFactory<T> where T : class
    {
        public string RootUri { get; private set; }

        private IWebHost host;
 
        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            this.host = builder.Build();
            this.host.Start();
            RootUri = this.host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault();

            return new TestServer(new WebHostBuilder().UseStartup<T>());
        }
 
        protected override void Dispose(bool disposing) 
        {
            base.Dispose(disposing);
            if (disposing) {
                this.host.Dispose();
            }
        }

        public void StartupServerInstance()
        {
            this.CreateClient();
        }
    }
}