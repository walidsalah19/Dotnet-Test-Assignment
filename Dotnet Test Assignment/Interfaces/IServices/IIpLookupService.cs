using Dotnet_Test_Assignment.Models;

namespace Dotnet_Test_Assignment.Interfaces.IServices
{
    public interface IIpLookupService
    {
        Task<IpLookupResult> LookupIpAsync(string ipAddress);
        Task<string> GetCountryCodeByIpAsync(string ipAddress);
    }
}
