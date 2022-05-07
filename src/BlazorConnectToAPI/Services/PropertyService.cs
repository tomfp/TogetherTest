using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharedDto;

namespace BlazorConnectToAPI.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public PropertyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<SoldPriceResult>> GetSoldPrices(string postcode)
        {
            var resultSet = new List<SoldPriceResult>();
            var httpClient = _httpClientFactory.CreateClient("PropertyApi");

            var response = await httpClient.GetAsync($"SoldPrice?postcode={Uri.EscapeDataString(postcode)}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                resultSet = JsonConvert.DeserializeObject<List<SoldPriceResult>>(content);
            }
            return resultSet;
        }
    }
}
