using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Skillmea
{
    public class TimerFunction
    {
        private readonly ILogger _logger;

        public TimerFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TimerFunction>();
        }

        [Function("TimerFunction")]
        public void Run([TimerTrigger("%TimerSchedule%")] TimerInfo myTimer)
        {
            _logger.LogInformation($"{Environment.GetEnvironmentVariable("TimerGreeting")}. C# Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
