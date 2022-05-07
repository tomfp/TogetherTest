using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using PropertyApi.Config;
using PropertyApi.Services;

namespace PropertyApi.Test
{
    [TestClass]
    public class PostcodeServiceTests
    {
        [TestMethod]
        public async Task PostcodeService_ValidPostcode_APICall_Returns_True()
        {
            var factoryMock = new Mock<IHttpClientFactory>();
            var exampleReturnedContent = "{\"status\":200,\"result\":{\"postcode\":\"SW1A 1AA\",\"quality\":1,\"eastings\":391198,\"northings\":374256,\"country\":\"England\",\"nhs_ha\":\"North West\",\"longitude\":-2.133423,\"latitude\":53.265225,\"european_electoral_region\":\"North West\",\"primary_care_trust\":\"Central and Eastern Cheshire\",\"region\":\"North West\",\"lsoa\":\"Cheshire East 017A\",\"msoa\":\"Cheshire East 017\",\"incode\":\"3AQ\",\"outcode\":\"SK10\",\"parliamentary_constituency\":\"Macclesfield\",\"admin_district\":\"Cheshire East\",\"parish\":\"Macclesfield\",\"admin_county\":null,\"admin_ward\":\"Macclesfield Tytherington\",\"ced\":null,\"ccg\":\"NHS Cheshire\",\"nuts\":\"Cheshire East\",\"codes\":{\"admin_district\":\"E06000049\",\"admin_county\":\"E99999999\",\"admin_ward\":\"E05008638\",\"parish\":\"E04012471\",\"parliamentary_constituency\":\"E14000802\",\"ccg\":\"E38000233\",\"ccg_id\":\"27D\",\"ced\":\"E99999999\",\"nuts\":\"TLD62\",\"lsoa\":\"E01018615\",\"msoa\":\"E02003869\",\"lau2\":\"E06000049\"}}}";

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(exampleReturnedContent),
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://doesnt.matter.never/called"),
            };

            factoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            var subjectUnderTest = new PostcodeService(factoryMock.Object);
            var postcode = "SW1A 1AA";
            var result = await subjectUnderTest.ValidatePostcode(postcode);
            Assert.AreEqual(postcode, result);
        }

        [TestMethod]
        public async Task PostcodeService_InvalidPostcode_APIReturns_False()
        {
            var factoryMock = new Mock<IHttpClientFactory>();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://doesnt.matter.never/called"),
            };

            factoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            var subjectUnderTest = new PostcodeService(factoryMock.Object);
            var postcode = "WX22 9OK";
            var result = await subjectUnderTest.ValidatePostcode(postcode);
            Assert.IsNull(result);
        }
    }

}
