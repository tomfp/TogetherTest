namespace PropertyApi.Services
{
    public interface IPostcodeService
    {
        Task<string?> ValidatePostcode(string postcode);
    }
}
