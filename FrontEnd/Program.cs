namespace FrontEnd
{
    using Backend;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(s =>
                    s.AddSingleton<IBackEndFacade>(p => new BackEndFacade()))
                .UseStartup<Startup>();
    }
}
