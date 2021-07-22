using System;
using System.Net.Http;
using System.Threading.Tasks;
using BLG.Business;
using BLG.Business.Interfaces;
using BLGWeather.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BLGWeather.Web.Tests
{
    public class FakeHttpClient : IHttpClient
    {
        public HttpClient HttpRequestClient { get; set; }
    }
    [TestClass]
    public class BLGBusinessUnitTests
    {
        private IWeatherRequests whetherRequests;
           [TestInitialize]
        public void Setup()
        {
            var fakeHttpClient = new FakeHttpClient();
            fakeHttpClient.HttpRequestClient = null;

            var moqRequests = new Mock<BLGLocationWeatherRequests>(fakeHttpClient,"FakeApiKey");
            moqRequests.Setup(q => q.GetLocationWeather(It.IsAny<String>(), It.IsAny<Location>())).Returns(Task.FromResult(new LocationWeatherElements { }));
            whetherRequests = moqRequests.Object;
        }
        [TestMethod]
        public void Test_BLGLocationWeatherRequests_GetLocationWeather_DoesNot_Throw_Exceptions()
        {
            var result = whetherRequests.GetLocationWeather("fakeUrl", new Location { CityName = "Lambar", CountryCode = "LandOfMasses" }).ConfigureAwait(true).GetAwaiter().GetResult();
            Assert.IsInstanceOfType(result, typeof(LocationWeatherElements));
        }
    }
}
