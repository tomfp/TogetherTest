using System.ComponentModel;
using System.Dynamic;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PropertyApi.Config;
using SharedDto;
namespace PropertyApi.Services
{
    public class SoldPriceRepository : ISoldPriceRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SoldPriceRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // land Registry Search, example queryString
        // et%5B%5D=lrcommon%3Afreehold&et%5B%5D=lrcommon%3Aleasehold&limit=all&nb%5B%5D=true&nb%5B%5D=false&postcode=B27+7HA&ptype%5B%5D=lrcommon%3Adetached&ptype%5B%5D=lrcommon%3Asemi-detached&ptype%5B%5D=lrcommon%3Aterraced&ptype%5B%5D=lrcommon%3Aflat-maisonette&ptype%5B%5D=lrcommon%3AotherPropertyType&tc%5B%5D=ppd%3AstandardPricePaidTransaction&tc%5B%5D=ppd%3AadditionalPricePaidTransaction
        // et[]=lrcommon:freehold&et[]=lrcommon:leasehold&limit=all&nb[]=true&nb[]=false&postcode=B1+1AA&ptype[]=lrcommon:detached&ptype[]=lrcommon:semi-detached&ptype[]=lrcommon:terraced&ptype[]=lrcommon:flat-maisonette&ptype[]=lrcommon:otherPropertyType&tc[]=ppd:standardPricePaidTransaction&tc[]=ppd:additionalPricePaidTransaction
        // TODO find how to order by most recent, and limit search to maybe 20 results
        public async Task<IEnumerable<SoldPriceResult>> GetSoldPriceByPostcode(string postcode)
        {
            var resultSet = new List<SoldPriceResult>();
            var httpClient = _httpClientFactory.CreateClient(ConfigValues.SearchPriceClientName);
            var url = $"ppd_data.csv?&postcode={HttpUtility.UrlEncode(postcode)}";
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                using var textReader = new StringReader(content);
                // format is in array of string arrays - all quoted
                // columns are 0 - SaleId, 1 price, 2 - saleDate, 3 - postcode,4,5,6,7 - house No, 8 - address1, 9 - address2, 10 - address3, 11 - address4, 12 - address5
                while(true)
                {
                    var line = textReader.ReadLine();
                    if (line == null) { break;}
                    var splits = line.Replace("\"", "").Split(',');
                    if (splits.Length > 14)
                    {
                        resultSet.Add(new SoldPriceResult
                        {
                            Id = splits[0],
                            Price = splits[1],
                            DateOfSale = splits[2],
                            Postcode = splits[3],
                            HouseNumber = splits[8],
                            Address1 = splits[9],
                            Address2 = splits[10],
                            Address3 = splits[11],
                            Address4 = splits[12],
                            Address5 = splits[13],
                            SaleDetailsUrl = splits[15]
                        });
                    }
                }
            }
            return resultSet;
        }
    }
}
