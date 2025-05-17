using Dotnet_Test_Assignment.Interfaces.IRepoInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_Test_Assignment.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogRepository _logService;

        public LogsController(ILogRepository logService)
        {
            _logService = logService;
        }

        [HttpGet("blocked-attempts")]
        public IActionResult GetLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var logs = _logService.GetAllLogs(page, pageSize);
            return Ok(logs);
        }
    }
}
