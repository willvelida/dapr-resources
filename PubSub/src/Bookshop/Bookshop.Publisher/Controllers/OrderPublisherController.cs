using Bookshop.Common;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Publisher.Controllers
{
    [Route("api/orderpublisher")]
    [ApiController]
    public class OrderPublisherController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger _logger;

        public OrderPublisherController(DaprClient daprClient, ILogger logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order is not null)
            {
                _logger.LogInformation($"Publishing order ID {order.OrderId}");

                await _daprClient.PublishEventAsync("dapr-pubsub-servicebus", "", order);

                return Created($"api/orderpublisher/{order.OrderId}", order);
            }

            return BadRequest();
        }
    }
}
