using Dotnet_Test_Assignment.Models;

namespace Dotnet_Test_Assignment.Interfaces.IRepoInterfaces
{
    public interface ILogRepository
    {
        void LogAttempt(LogsAttemps log);
        IEnumerable<LogsAttemps> GetAllLogs(int page = 1, int pageSize = 10);
    }
}
