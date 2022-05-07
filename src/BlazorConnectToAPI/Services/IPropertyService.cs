using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedDto;

namespace BlazorConnectToAPI.Services
{
    public interface IPropertyService
    {
        Task<IEnumerable<SoldPriceResult>> GetSoldPrices(string postcode);
    }
}
