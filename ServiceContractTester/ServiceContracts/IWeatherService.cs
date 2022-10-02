using Common;

namespace ServiceContracts
{
    public interface IWeatherService
    {
        [ServiceMethodExport]
        decimal GetCurrentTemperature(LocationSearchDto locationSearch);

        [ServiceMethodExport]
        ForcastDto GetForcast(LocationSearchDto locationSearch, string city);

        string NonExportedMethod();

        [ServiceMethodExport]
        string FalselyExportedMethod();
    }

    public class LocationSearchDto
    {
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    public class ForcastDto
    {
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public string Comment { get; set; }
    }
}