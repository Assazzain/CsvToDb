using csv_to_database.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace csv_to_database.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CSVController : ControllerBase
    {
        private ICSVService _csvService;
        public CSVController(ICSVService csvService)
        {
            _csvService = csvService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            await _csvService.Convert(file);
            return Ok(file);
        }
    }
}
