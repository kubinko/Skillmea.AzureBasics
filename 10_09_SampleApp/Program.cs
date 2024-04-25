using Microsoft.ApplicationInsights.Extensibility;
using SampleApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationInsightsTelemetry()
    .AddApplicationInsightsTelemetryProcessor<MyTelemetryProcessor>()
    .AddSingleton<ITelemetryInitializer, MyTelemetryInitializer>();

var app = builder.Build();

app.MapGet("/", () => $"Good morning friend!");
app.MapGet("/health", () => "Healthy");
app.MapGet("/information", (string message) => app.Logger.LogInformation(message));
app.MapGet(
    "/error",
    (string message) => {
        throw new InvalidOperationException(message);
    }
);
app.MapPost("/person", (SamplePersonData data) => Results.Created());

app.Run();

class SamplePersonData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}