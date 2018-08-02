namespace FrontEnd.Courses
{
    using System;
    using System.Net;
    using System.Net.Http;
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
        private FakeGuidGenerator guidGenerator;

        [Background]
        public void Background()
        {
            "es existiert ein Server".x(() =>
            {
                this.guidGenerator = new FakeGuidGenerator();
                this.backend = new FakeBackEndFacade();
                var serverHostBuilder = new WebHostBuilder();
                serverHostBuilder
                    .ConfigureServices(s =>
                        s
                            .AddSingleton<IGuidGenerator>(p => this.guidGenerator)
                            .AddSingleton<IBackEndFacade>(p => this.backend))
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
            var id = Guid.Parse("6C9E6EC4-8E28-4E88-8184-351729302580");
            var body = $@"{{
                'name': '{name}'
            }}";

            "_".x(()
                => this.guidGenerator.SetNext(id));

            "wenn auf den Server ein neuer Kurs gepostet wird".x(async ()
                => response = await this.client.Post(
                    "api/courses",
                    body));

            "soll die Page erreichbar sein".x(()
                => response.StatusCode.Should().Be(HttpStatusCode.OK));

            "soll der Namen an das BackEnd übergeben werden".x(()
                => this.backend
                    .CreateCourseName.Should().Be(name));

            "soll eine ID an das BackEnd übergeben werden".x(()
                => this.backend
                    .CreateCourseId.Should().Be(id));
        }
    }
}