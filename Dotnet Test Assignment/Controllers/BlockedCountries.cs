using Dotnet_Test_Assignment.Interfaces.IRepoInterfaces;
using Dotnet_Test_Assignment.Interfaces.IServices;
using Dotnet_Test_Assignment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_Test_Assignment.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class BlockedCountries : ControllerBase
    {
        private readonly IBlockedCountryRepository _repo;
        private readonly ITemporaryBlockService temporaryService;

        public BlockedCountries(IBlockedCountryRepository repo, ITemporaryBlockService temporaryService)
        {
            _repo = repo;
            this.temporaryService = temporaryService;
        }

        [HttpPost("block")]
        public IActionResult Block([FromBody] string countryCode)
        {
            if (!_repo.Add(countryCode))
                return Conflict("Already blocked.");

            return Ok("Blocked");
        }

        [HttpDelete("block/{countryCode}")]
        public IActionResult Unblock(string countryCode)
        {
            if (!_repo.Remove(countryCode))
                return NotFound("Not found.");

            return Ok("Unblocked");
        }

        [HttpGet("blocked")]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var all = _repo.GetAll();
            var paged = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(paged);
        }
        [HttpPost("temporal-block")]
        public IActionResult TempBlock([FromBody] TempBlockRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CountryCode))
                return BadRequest("Country code is required.");

            if (request.DurationMinutes < 1 || request.DurationMinutes > 1440)
                return BadRequest("Duration must be between 1 and 1440 minutes.");

            var success = temporaryService.TryAddTemporaryBlock(request.CountryCode, request.DurationMinutes);
            if (!success)
                return Conflict("Country already temporarily blocked.");

            return Ok("Temporary block added.");
        }
    }
}
