using Bookshop.Common;
using Microsoft.AspNetCore.Mvc;

namespace Bookshop.Subscriber.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderSubscriberController : ControllerBase
    {
        private readonly ILogger<OrderSubscriberController> _logger;

        public OrderSubscriberController(ILogger<OrderSubscriberController> logger)
        {
            _logger = logger;
        }

        [Dapr.Topic("dapr-pubsub", "orderstopic")]
        [HttpPost("orderreceived")]
        public IActionResult OrderReceived(Order order)
        {
            if (order is not null)
            {
                _logger.LogInformation($"Received Order ID: {order.OrderId}: {order.Title} - {order.Author} - {order.Price}");
                return Ok();
            }

            return BadRequest();
        }
    }
}
