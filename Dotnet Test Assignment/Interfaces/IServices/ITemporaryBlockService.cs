using Dotnet_Test_Assignment.Models;

namespace Dotnet_Test_Assignment.Interfaces.IServices
{
    public interface ITemporaryBlockService
    {
        bool TryAddTemporaryBlock(string countryCode, int durationMinutes);
        bool IsTemporarilyBlocked(string countryCode);
        void CleanupExpiredBlocks();
        IEnumerable<TemporaryBlockEntry> GetAll();
    }
}
