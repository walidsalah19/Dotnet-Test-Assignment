using Dotnet_Test_Assignment.Interfaces.IServices;
using Dotnet_Test_Assignment.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Dotnet_Test_Assignment.Services
{
    public class IpLookupService: IIpLookupService
    {
    
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "059f4eb7c17747bba4dc67523e94a0b8";

        public IpLookupService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IpLookupResult> LookupIpAsync(string ipAddress)
        {
            if (!IsValidIp(ipAddress))
                throw new ArgumentException("Invalid IP address format");

            var url = $"https://api.ipgeolocation.io/ipgeo?apiKey={_apiKey}&ip={ipAddress}";

            var httpResponse = await _httpClient.GetAsync(url);

            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception($"IP lookup failed with status code {httpResponse.StatusCode}");

            var content = await httpResponse.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<IpApiResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (response == null || string.IsNullOrWhiteSpace(response.CountryName))
                throw new Exception("Failed to fetch IP details from API");

            return new IpLookupResult
            {
                IpAddress = response.Ip,
                CountryCode = response.CountryCode,
                CountryName = response.CountryName,
                Isp = response.Isp
            };
        }

        public async Task<string> GetCountryCodeByIpAsync(string ipAddress)
        {
            var lookup = await LookupIpAsync(ipAddress);
            return lookup.CountryCode;
        }

        private bool IsValidIp(string ip)
        {
            // Basic IPv4/IPv6 validation
            if (string.IsNullOrWhiteSpace(ip)) return false;

            var pattern =
                @"^(([0-9]{1,3}\.){3}[0-9]{1,3})$|^(([0-9a-fA-F]{0,4}:){1,7}[0-9a-fA-F]{0,4})$";

            return Regex.IsMatch(ip, pattern);
        }

      
    }
}
