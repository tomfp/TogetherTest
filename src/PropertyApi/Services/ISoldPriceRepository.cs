using SharedDto;
namespace PropertyApi.Services
{
    public interface ISoldPriceRepository
    {
        Task<IEnumerable<SoldPriceResult>> GetSoldPriceByPostcode(string postcode);

    }
}
