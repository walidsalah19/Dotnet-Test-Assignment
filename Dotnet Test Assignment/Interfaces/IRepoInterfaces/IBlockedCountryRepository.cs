namespace Dotnet_Test_Assignment.Interfaces.IRepoInterfaces
{
    public interface IBlockedCountryRepository
    {
        bool Add(string countryCode);
        bool Remove(string countryCode);
        bool IsBlocked(string countryCode);
        List<string> GetAll();
    }
}
