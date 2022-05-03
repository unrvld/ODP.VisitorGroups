#if NET5_0
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using UNRVLD.ODP.VisitorGroups.REST.Models;

namespace UNRVLD.ODP.VisitorGroups.REST
{
    public class CustomerPropertyListRetriever : ICustomerPropertyListRetriever
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OdpVisitorGroupOptions _options;

        public CustomerPropertyListRetriever(IHttpClientFactory httpClientFactory, OdpVisitorGroupOptions options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options;
        }

        //public CustomerPropertyListRetriever()
        //{
        //    _httpClientFactory = ServiceLocator.Current.GetInstance<IHttpClientFactory>();
        //    _options = ServiceLocator.Current.GetInstance<OdpVisitorGroupOptions>();
        //}

        public async Task<IEnumerable<Field>> GetCustomerPropertiesAsync()
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                _options.BaseEndPoint + "/v3/schema/objects/customers")
            {
                Headers =
                {
                    { "x-api-key", _options.PrivateApiKey }
                }
            };

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                var customerFields = await JsonSerializer.DeserializeAsync
                    <CustomerFieldsResponse>(contentStream);

                if (customerFields != null)
                {
                    return customerFields.fields;
                }
            }

            return null;
        }

    }
}
#endif