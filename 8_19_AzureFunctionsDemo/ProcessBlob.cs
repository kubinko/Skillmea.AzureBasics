using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Skillmea
{
    public class ProcessBlob
    {
        private readonly ILogger<ProcessBlob> _logger;

        public ProcessBlob(ILogger<ProcessBlob> logger)
        {
            _logger = logger;
        }

        [Function(nameof(ProcessBlob))]
        public async Task Run([BlobTrigger("test/{name}")] Person person, string name)
        {
            _logger.LogInformation($"Name: {person.Name} Age: {person.Age} Address: {person.Address}");
        }
    }

    public class Person
    {
        public string Name {get; set;} = string.Empty;
        public int Age {get; set;}
        public string Address {get;set;} = string.Empty;
    }
}
