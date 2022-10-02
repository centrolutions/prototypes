using ServiceContracts;

namespace Services
{
    public class WeatherService : IWeatherService
    {
        public string FalselyExportedMethod()
        {
            throw new NotImplementedException();
        }

        public decimal GetCurrentTemperature(LocationSearchDto locationSearch)
        {
            throw new NotImplementedException();
        }

        public ForcastDto GetForcast(LocationSearchDto locationSearch, string city)
        {
            throw new NotImplementedException();
        }

        public string NonExportedMethod()
        {
            throw new NotImplementedException();
        }
    }
}