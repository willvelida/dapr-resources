using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Post()
        {
            _logger.LogInformation($"Hello Dapr Cron Job! The time is {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}");

            return Ok();
        }
    }
}
