using Bookshop.Common;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Subscriber.Controllers
{
    [Route("api/ordersubscriber")]
    [ApiController]
    public class OrderSubscriberController : ControllerBase
    {
        private readonly ILogger _logger;

        public OrderSubscriberController(ILogger logger)
        {
            _logger = logger;
        }

        [Dapr.Topic("dapr-pubsub-service", "orderreceivedtopic")]
        public async Task<IActionResult> OrderReceived([FromBody] Order order)
        {
            _logger.LogInformation($"Receiving order ID: {order.OrderId}");

            return Ok(order);
        }
    }
}
