using Bookshop.Common;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Publisher.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderPublisherController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<OrderPublisherController> _logger;

        public OrderPublisherController(DaprClient daprClient, ILogger<OrderPublisherController> logger)
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

                await _daprClient.PublishEventAsync("dapr-pubsub", "orderstopic", order);

                return Ok(order);
            }

            return BadRequest();
        }
    }
}
