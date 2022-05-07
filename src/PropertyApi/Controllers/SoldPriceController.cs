using Microsoft.AspNetCore.Mvc;
using PropertyApi.Services;
using SharedDto;
namespace PropertyApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SoldPriceController : ControllerBase
{

    private readonly ILogger<SoldPriceController> _logger;
    private readonly ISoldPriceRepository _soldPriceRepository;
    private readonly IPostcodeService _postcodeService;

    public SoldPriceController(ILogger<SoldPriceController> logger,
        ISoldPriceRepository soldPriceRepository,
        IPostcodeService postcodeService)
    {
        _logger = logger;
        _soldPriceRepository = soldPriceRepository;
        _postcodeService = postcodeService;
    }

    [HttpGet(Name = "GetSoldPricesByPostcode")]
    public async Task<IActionResult> GetByPostcode([FromQuery] string postcode)
    {
        try
        {
            var validatedPostcode = await _postcodeService.ValidatePostcode(postcode);
            if (string.IsNullOrWhiteSpace(validatedPostcode))
            {
                return BadRequest($"Invalid postcode {postcode}");
            }
            var result = await _soldPriceRepository.GetSoldPriceByPostcode(validatedPostcode);
            if (!result.Any())
            {
                return NotFound($"No prices found for postcode {validatedPostcode}");
            }
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error searching postcode value {postcode}");
            return StatusCode(500);
        }
    }
}