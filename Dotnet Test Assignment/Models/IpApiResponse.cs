using System.Text.Json.Serialization;

namespace Dotnet_Test_Assignment.Models
{
    public class IpApiResponse
    {
        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("country_code2")]
        public string CountryCode { get; set; }

        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }

        [JsonPropertyName("isp")]
        public string Isp { get; set; }
    }
}
