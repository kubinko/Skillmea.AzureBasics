using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Skillmea
{
    public class Hello
    {
        private readonly ILogger<Hello> _logger;

        public Hello(ILogger<Hello> logger)
        {
            _logger = logger;
        }

        [Function("Hello")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "shop/product/{productId:int}")] HttpRequest req,
            int productId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult($"Requested product ID: {productId}");
        }
    }
}
