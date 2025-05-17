using Dotnet_Test_Assignment.Interfaces.IRepoInterfaces;
using Dotnet_Test_Assignment.Interfaces.IServices;
using Microsoft.Extensions.Caching.Memory;

namespace Dotnet_Test_Assignment.Repostories
{
    public class BlockedCountryRepository : IBlockedCountryRepository
    {
        private readonly IMemoryCache _cache;
        private const string CACHE_KEY = "blocked_countries";
        private readonly ITemporaryBlockService temporaryService;

        public BlockedCountryRepository(IMemoryCache cache, ITemporaryBlockService temporaryService)
        {
            this.temporaryService = temporaryService;
            _cache = cache;

            // Ensure the cache is initialized
            if (!_cache.TryGetValue(CACHE_KEY, out HashSet<string> _))
            {
                _cache.Set(CACHE_KEY, new HashSet<string>(), new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(1)
                });
            }
        }

        public bool Add(string countryCode)
        {
            var list = _cache.Get<HashSet<string>>(CACHE_KEY);
            return list.Add(countryCode.ToUpper());
        }

        public bool Remove(string countryCode)
        {
            var list = _cache.Get<HashSet<string>>(CACHE_KEY);
            return list.Remove(countryCode.ToUpper());
        }

        public bool IsBlocked(string countryCode)
        {
            var list = _cache.Get<HashSet<string>>(CACHE_KEY);
            return list.Contains(countryCode.ToUpper())||temporaryService.IsTemporarilyBlocked(countryCode); ;
        }

        public List<string> GetAll(String filter="")
        {
            var blocked = _cache.Get<HashSet<string>>(CACHE_KEY);
            var tempBlocked = temporaryService.GetAll();
            var combined = blocked
            .Concat(tempBlocked.Select(t=>t.CountryCode))
             .GroupBy(c => c.ToUpper())
             .Select(g => g.First())
              .ToList(); ;

            return string.IsNullOrEmpty(filter)?  combined.ToList(): combined.Where(c => c.Equals(filter)).ToList();
        }
    }
}
