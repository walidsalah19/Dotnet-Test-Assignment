using Dotnet_Test_Assignment.Interfaces.IServices;
using Dotnet_Test_Assignment.Models;
using System.Collections.Concurrent;

namespace Dotnet_Test_Assignment.Services
{
    public class TemporaryBlockService: ITemporaryBlockService
    {
        private readonly ConcurrentDictionary<string, TemporaryBlockEntry> _tempBlocked = new();

        public bool TryAddTemporaryBlock(string countryCode, int durationMinutes)
        {
            if (_tempBlocked.ContainsKey(countryCode))
                return false;

            var entry = new TemporaryBlockEntry
            {
                CountryCode = countryCode.ToUpper(),
                ExpiryTime = DateTime.UtcNow.AddMinutes(durationMinutes)
            };

            return _tempBlocked.TryAdd(entry.CountryCode, entry);
        }

        public bool IsTemporarilyBlocked(string countryCode)
        {
            if (!_tempBlocked.TryGetValue(countryCode.ToUpper(), out var entry))
                return false;

            if (entry.ExpiryTime < DateTime.UtcNow)
            {
                _tempBlocked.TryRemove(countryCode.ToUpper(), out _);
                return false;
            }

            return true;
        }

        public void CleanupExpiredBlocks()
        {
            var now = DateTime.UtcNow;
            foreach (var kvp in _tempBlocked)
            {
                if (kvp.Value.ExpiryTime < now)
                {
                    _tempBlocked.TryRemove(kvp.Key, out _);
                }
            }
        }

        public IEnumerable<TemporaryBlockEntry> GetAll()
        {
            return _tempBlocked.Values.OrderBy(e => e.ExpiryTime);
        }
    }
}
