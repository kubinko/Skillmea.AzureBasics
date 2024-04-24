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

        [Function("ProtectedByFunctionKey1")]
        public IActionResult ProtectedByFunctionKey1(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
            => new OkObjectResult($"First function protected by function key");

        [Function("ProtectedByFunctionKey2")]
        public IActionResult ProtectedByFunctionKey2(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
            => new OkObjectResult($"Second function protected by function key");

        [Function("ProtectedByAdminKey")]
        public IActionResult ProtectedByAdminKey(
            [HttpTrigger(AuthorizationLevel.Admin, "get", "post")] HttpRequest req)
            => new OkObjectResult($"Function protected by admin key");

        [Function("CreateBlob")]
        [BlobOutput("test/{name}", Connection = "AzureWebJobsStorage")]
        public Task<string> CreateBlob(
            [HttpTrigger(AuthorizationLevel.Admin, "post", Route = "{name}")] HttpRequest req,
            string name)
            => new StreamReader(req.Body).ReadToEndAsync();

        [Function("ReadBlob")]
        public IActionResult ReadBlob(
            [HttpTrigger(AuthorizationLevel.Admin, "get", Route = "{name}")] HttpRequest req,
            string name,
            [BlobInput("test/{name}", Connection = "AzureWebJobsStorage")] string inputBlob)
            => new OkObjectResult(inputBlob);
    }
}
