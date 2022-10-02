using Common;
using ServiceContracts;

namespace Services.Tests
{
    public class IWeatherServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InterfaceExportsMethodsCorrectly()
        {
            var tester = new ServiceMethodExportTester<IWeatherService>()
                .ShouldExport(nameof(IWeatherService.GetCurrentTemperature))
                    .WithParameter<LocationSearchDto>("locationSearch")
                .ShouldExport(nameof(IWeatherService.GetForcast))
                    .WithParameter<LocationSearchDto>("locationSearch")
                    .WithParameter<int>("city");

            tester.AssertAll();
        }
    }
}