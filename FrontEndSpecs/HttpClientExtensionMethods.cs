namespace FrontEnd
{
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

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

        public static async Task<Stream> GetContent(
            this HttpResponseMessage response)
        {
            return await response.Content.ReadAsStreamAsync();
        }
    }
}