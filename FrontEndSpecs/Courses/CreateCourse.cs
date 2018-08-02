namespace FrontEnd.Courses
{
    using System.Net;
    using System.Net.Http;
    using FluentAssertions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Xbehave;

    public class CreateCourse
    {
        private TestServer server;
        private HttpClient client;

        [Background]
        public void Background()
        {
            "es existiert ein Server".x(() =>
            {
                var serverHostBuilder = new WebHostBuilder();
                serverHostBuilder
                    .UseStartup<Startup>();

                this.server = new TestServer(serverHostBuilder);
                this.client = this.server.CreateClient();
            });
        }

        [Scenario]
        public void KursErstellen(
            HttpResponseMessage response)
        {
            "wenn ich den Server kontaktiere".x(async ()
                => response = await this.client.Get("something"));

            "soll die Page erreichbar sein".x(()
                => response.StatusCode.Should().Be(HttpStatusCode.OK));
        }
    }
}