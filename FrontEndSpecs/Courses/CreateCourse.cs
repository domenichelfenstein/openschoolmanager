namespace FrontEnd.Courses
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Backend;
    using Fakes;
    using FluentAssertions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Xbehave;

    public class CreateCourse
    {
        private TestServer server;
        private HttpClient client;
        private FakeBackEndFacade backend;

        [Background]
        public void Background()
        {
            "es existiert ein Server".x(() =>
            {
                this.backend = new FakeBackEndFacade();
                var serverHostBuilder = new WebHostBuilder();
                serverHostBuilder
                    .ConfigureServices(s =>
                        s.AddSingleton<IBackEndFacade>(p => this.backend))
                    .UseStartup<Startup>();

                this.server = new TestServer(serverHostBuilder);
                this.client = this.server.CreateClient();
            });
        }

        [Scenario]
        public void KursErstellen(
            HttpResponseMessage response)
        {
            var name = "some name";
            var body = $@"{{
                'name': '{name}'
            }}";

            "wenn auf den Server ein neuer Kurs gepostet wird".x(async ()
                => response = await this.client.Post(
                    "api/courses",
                    body));

            "soll die Page erreichbar sein".x(()
                => response.StatusCode.Should().Be(HttpStatusCode.OK));

            "soll das BackEnd aufgerufen worden sein".x(()
                => this.backend.CreateCourseName.Should().Be(name));
        }
    }
}