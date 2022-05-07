using System.Dynamic;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PropertyApi.Config;
namespace PropertyApi.Services
{
    public class PostcodeService : IPostcodeService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public PostcodeService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string?> ValidatePostcode(string postcode)
        {
            var httpClient = _httpClientFactory.CreateClient(ConfigValues.PostcodeClientName);
            var response = await httpClient.GetAsync(Uri.EscapeDataString(postcode));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dynamic? postcodeResult =
                    JsonConvert.DeserializeObject<ExpandoObject>(content,
                        new ExpandoObjectConverter());

                if (postcodeResult != null)
                {
                    var resultPostcode = ((dynamic)postcodeResult).result.postcode;
                    //TODO get location lat/long are available on this object
                    return resultPostcode;
                }
            }
            return null;
        }
    }
}
