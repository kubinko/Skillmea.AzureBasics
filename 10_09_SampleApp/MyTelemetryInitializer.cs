using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace SampleApp;

public class MyTelemetryInitializer : ITelemetryInitializer
{
    public void Initialize(ITelemetry telemetry)
    {
        if (telemetry is RequestTelemetry request)
        {
            if (int.TryParse(request.ResponseCode, out int code)
                && code >= 400 && code < 500)
            {
                request.Success = true;
            }

            request.Properties.Add("Custom property", Guid.NewGuid().ToString());
        }
    }
}