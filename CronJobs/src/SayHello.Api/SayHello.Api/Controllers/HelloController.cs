using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SayHello.Api.Controllers
{
    [Route("SayHello")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private readonly ILogger<HelloController> _logger;
        private readonly DaprClient _daprClient;

        public HelloController(ILogger<HelloController> logger, DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                _logger.LogInformation("Hello Dapr Cron Job!");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(Post)}: {ex.Message}");
                throw;
            }
        }
    }
}
