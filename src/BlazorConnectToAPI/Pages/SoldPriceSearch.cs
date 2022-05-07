using BlazorConnectToAPI.Services;

using Microsoft.AspNetCore.Components;

using SharedDto;

namespace BlazorConnectToAPI.Pages;

public partial class SoldPriceSearch
{
    [Inject]
    public IPropertyService PropertyService { get; set; }
    public IEnumerable<SoldPriceResult>? SoldResults { get; set; }

    public string? Postcode { get; set; }

    public string? Location { get; set; }
    public string? Message { get; set; }

    public async void GetResults()
    {
        SoldResults = null;
        Message = null; 
        if (!string.IsNullOrWhiteSpace(Postcode))
        {
            await LookupResults();
            Location = $"https://www.google.co.uk/maps/place/{Uri.EscapeDataString(Postcode)}";
            StateHasChanged();
        }
        else
        {
            Location = null;
            Message = "Please Enter a postcode";
        }
    }

    private async Task LookupResults()
    {
        try
        {
            var results = await PropertyService.GetSoldPrices(Postcode);
            var soldPriceResults = results.OrderByDescending(x=>x.DateOfSale).ToArray();
            if (soldPriceResults.Any())
            {
                SoldResults = soldPriceResults;
            }
            else
            {
                Message = $"No results found for postcode {Postcode}";
            }
        }
        catch (Exception ex)
        {
            Location = null;
            Message += $"Error loading cases: {ex.Message} {ex.StackTrace}";
        }
    }
}
