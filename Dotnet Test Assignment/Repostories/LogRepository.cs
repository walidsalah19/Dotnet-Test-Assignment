using Dotnet_Test_Assignment.Interfaces.IRepoInterfaces;
using Dotnet_Test_Assignment.Models;
using System.Collections.Concurrent;

namespace Dotnet_Test_Assignment.Repostories
{
    public class LogRepository : ILogRepository
    {
        private readonly ConcurrentBag<LogsAttemps> _logs = new();
        public void LogAttempt(LogsAttemps log)
        {
            _logs.Add(log);
        }

        public IEnumerable<LogsAttemps> GetAllLogs(int page = 1, int pageSize = 10)
        {
            return _logs.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
