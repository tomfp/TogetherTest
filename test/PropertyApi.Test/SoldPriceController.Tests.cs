using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PropertyApi.Controllers;
using PropertyApi.Services;
using SharedDto;

/* References
 
 https://gingter.org/2018/07/26/how-to-mock-httpclient-in-your-net-c-unit-tests/

 */


namespace PropertyApi.Test;

[TestClass]
public class SoldPriceControllerTests
{

    private readonly ILogger<SoldPriceController> _loggerMock = new Mock<ILogger<SoldPriceController>>().Object;

    [TestMethod]
    public async Task SoldPriceController_InvalidPostcode_ReturnsBadRequest()
    {
        var soldPriceServiceMock = new Mock<ISoldPriceRepository>();
        var invalidPostcodeServiceMock = Mock.Of<IPostcodeService>(
            c=>c.ValidatePostcode(It.IsAny<string>()) == Task.FromResult(string.Empty));

        var subjectUnderTest = new SoldPriceController(
            _loggerMock,
            soldPriceServiceMock.Object,
            invalidPostcodeServiceMock);

        var result = await subjectUnderTest.GetByPostcode(string.Empty);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task SoldPriceController_InvalidPostcode_DoesNotCall_SearchPriceService()
    {
        var soldPriceServiceMock = new Mock<ISoldPriceRepository>();
        var invalidPostcodeServiceMock = Mock.Of<IPostcodeService>(
            c => c.ValidatePostcode(It.IsAny<string>()) == Task.FromResult(string.Empty));

        var subjectUnderTest = new SoldPriceController(
            _loggerMock,
            soldPriceServiceMock.Object,
            invalidPostcodeServiceMock);

        var result = await subjectUnderTest.GetByPostcode(null);
        soldPriceServiceMock.Verify(t => t.GetSoldPriceByPostcode(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task SoldPriceController_ValidPostcode_Calls_SearchPriceService()
    {
        var soldPriceServiceMock = new Mock<ISoldPriceRepository>();
        var validPostcodeServiceMock = Mock.Of<IPostcodeService>(
            c => c.ValidatePostcode(It.IsAny<string>()) == Task.FromResult("SW1 1AA"));

        var subjectUnderTest = new SoldPriceController(
            _loggerMock,
            soldPriceServiceMock.Object,
            validPostcodeServiceMock);

        var result = await subjectUnderTest.GetByPostcode("SW1 1AA");
        soldPriceServiceMock.Verify(t=>t.GetSoldPriceByPostcode(It.IsAny<string>()));
    }

}