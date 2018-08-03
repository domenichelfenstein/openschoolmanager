namespace FrontEnd
{
    using System;
    using System.Linq;
    using Backend;
    using Fakes;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Server.Features;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;

    public class SeleniumServerFactory<T> : WebApplicationFactory<T> where T : class
    {
        private readonly Action<IServiceCollection> configServices;
        public string RootUri { get; private set; }

        private IWebHost host;

        public SeleniumServerFactory(
            Action<IServiceCollection> configServices)
        {
            this.configServices = configServices;
        }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            this.host = builder
                .ConfigureServices(this.configServices)
                .Build();
            this.host.Start();
            RootUri = this.host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault();

            return new TestServer(
                new WebHostBuilder()
                    .UseStartup<T>());
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