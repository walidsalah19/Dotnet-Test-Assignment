using Dotnet_Test_Assignment.Interfaces.IServices;
using Dotnet_Test_Assignment.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Dotnet_Test_Assignment.Services
{
    public class IpLookupService: IIpLookupService
    {
    
        private readonly HttpClient _httpClient;

        public IpLookupService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IpLookupResult> LookupIpAsync(string ipAddress)
        {
            var url = $"https://ipapi.co/{ipAddress}/json/";

            var httpResponse = await _httpClient.GetAsync(url);

            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception($"IP lookup failed with status code {httpResponse.StatusCode}");

            var content = await httpResponse.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<IpApiResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (response == null || string.IsNullOrWhiteSpace(response.country))
                throw new Exception("Failed to fetch IP details from API");

            return new IpLookupResult
            {
                IpAddress = response.ip,
                CountryCode = response.country,
                CountryName = response.country_name,
                Isp = response.org
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
