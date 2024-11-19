using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebApplication3.Services;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeoutController : ControllerBase
    {
        private readonly CacheService _cacheService;

        public delegate void CalculationCompletedHandler(string cacheKey, int result, int delay);
        public event CalculationCompletedHandler? OnCalculationCompleted;

        public TimeoutController(CacheService cacheService)
        {
            _cacheService = cacheService;
            OnCalculationCompleted += _cacheService.WriteResultToCache;
        }


        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate([FromQuery] int number1, [FromQuery] int number2)
        {
            if (number1 < 1 || number1 > 3)
            {
                return BadRequest("Number1 should be 1, 2 or 3");
            }



            int delay = number1 switch
            {
                1 => 30000,
                2 => 45000,
                3 => 60000,
                _ => throw new InvalidOperationException("Invalid value")
            };

            await Task.Delay(delay);

            int result = number1 * number2;

            OnCalculationCompleted?.Invoke("last-result", result, delay);

            return Ok(new {message = "The result is written to the cache" });
        }

        [HttpGet("get-result")]
        public IActionResult GetResult()
        {
            if (_cacheService.TryGetValue("last-result", out int result))
            {
                return Ok(new { result });
            }

            return NotFound(new { message = "Nothing found in the cache" });
        }

    }


}
