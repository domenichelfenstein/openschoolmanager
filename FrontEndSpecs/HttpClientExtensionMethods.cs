namespace FrontEnd
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using FluentAssertions.Collections;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public static class HttpClientExtensionMethods
    {
        public static async Task<HttpResponseMessage> Get(
            this HttpClient client,
            string urlPart)
        {
            var response = await client.GetAsync(urlPart);

            return response;
        }

        public static async Task<HttpResponseMessage> Post(
            this HttpClient client,
            string urlPart,
            string body)
        {
            var response = await client.PostAsync(
                urlPart,
                new StringContent(
                    body,
                    Encoding.UTF8,
                    "application/json"));

            return response;
        }

        public static async Task<JToken> GetJson(
            this HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JToken.Parse(content);
        }

        public static async Task<string> GetContent(
            this HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<AndConstraint<GenericCollectionAssertions<JToken>>> ShouldBeJson(
            this Task<string> contentTask,
            string other)
        {
            var content = await contentTask;
            var jContent = JToken.Parse(content);
            var jOther = JToken.Parse(other);
            return jContent.Should().BeEquivalentTo(jOther);
        }
    }
}