using System;
using BLG.Business;
using BLG.Business.Interfaces;
using BLGWeather.Domain;
using BLGWeather.WebApi.Controllers.BLGWeather.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http;

namespace BLGWeather.Web.Tests
{
    [TestClass]
    public class LocationWeatherControllerIntegrationUnitTests
    {
        private IWeatherRequests whetherRequests;
        private readonly string ApiKey = "af9ec052cf8b47b1bce82bf0a4e40c6f";
        private readonly string baseUrl = "https://api.openweathermap.org/data/2.5/weather?q=";
        private LocationWeatherElements locationWeatherElements;
        private LocationWeatherController locationWeatherController;
        private BLGLocationWeatherRequests requests;
        [TestInitialize]
        public void Setup()
        {
            locationWeatherController = new LocationWeatherController();
            locationWeatherElements = new LocationWeatherElements
            {
                Humidity = 0,
                Location = new Location { CityName = "London", CountryCode = "UK" },
                Pressure = 0,
                Sunrise = DateTime.Now,
                Sunset = DateTime.Now,
                Temperature = new Temperature { CurrentTemperature = 0, MaximumTemperature = 0, MinmumTemperature = 0 }
            };

            var realHttpClient = new FakeHttpClient();
            realHttpClient.HttpRequestClient = new System.Net.Http.HttpClient();

            requests = new BLGLocationWeatherRequests(realHttpClient, ApiKey);
        }
        [TestMethod]
        public void Test_BLGLocationWeatherRequests_GetLocationWeather_Returns_IHttpActionResult()
        {
            var result = locationWeatherController.GetLocationWeather("London","UK").ConfigureAwait(true).GetAwaiter().GetResult();
            Assert.IsTrue(typeof(IHttpActionResult).IsAssignableFrom(result.GetType()));
        }

        [TestMethod]
        public void Test_BLGLocationWeatherRequests_GetLocationWeather_London_Uk_Returns_CorrectLocation()
        {
            var result = requests.GetLocationWeather(baseUrl,new Location { CityName="London", CountryCode="UK"}).ConfigureAwait(true).GetAwaiter().GetResult();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Location.CityName);
            Assert.AreEqual(result.Location.CityName.ToLower(),"London".ToLower());
        }
    }
}
