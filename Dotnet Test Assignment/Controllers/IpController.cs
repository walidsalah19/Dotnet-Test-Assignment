using Dotnet_Test_Assignment.Interfaces.IRepoInterfaces;
using Dotnet_Test_Assignment.Interfaces.IServices;
using Dotnet_Test_Assignment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_Test_Assignment.Controllers
{
    [Route("api/ip")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly IIpLookupService _ipLookupService;
        private readonly IBlockedCountryRepository _blockedService;
        private readonly ILogRepository _logService;
        public IpController(IIpLookupService ipLookupService, IBlockedCountryRepository blockedService, ILogRepository logService)
        {
            _ipLookupService = ipLookupService;
            _blockedService = blockedService;
            _logService = logService;
        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = Request.Headers["User-Agent"].ToString();
            if (ip == "::1")
            {
                ip = "41.68.2.24"; 
            }
            var countryCode = await _ipLookupService.GetCountryCodeByIpAsync(ip);
            var isBlocked = _blockedService.IsBlocked(countryCode);
            var model = new LogsAttemps
            {
                IpAddress = ip,
                CountryCode = countryCode,
                Timestamp = DateTime.UtcNow,
                IsBlocked = isBlocked,
                UserAgent = userAgent
            };

            _logService.LogAttempt(model);

            return Ok(model);
        }
        [HttpGet("lookup")]
        public async Task<IActionResult> LookupIp([FromQuery] string? ipAddress)
        {
            string ipToLookup = ipAddress ?? HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

            Console.WriteLine("Raw Response: " + ipToLookup);
            try
            {
                var result = await _ipLookupService.LookupIpAsync(ipToLookup);

                Console.WriteLine("Raw Response: " + result);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Failed to lookup IP"+ e.Message);
            }
        }

    }
}
