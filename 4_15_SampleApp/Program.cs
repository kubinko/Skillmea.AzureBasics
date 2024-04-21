var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => $"Good morning {Environment.GetEnvironmentVariable("TODAY_NAMESDAY")} ({Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")})!");
app.MapGet("/health", () => "Healthy!");
app.MapGet("/information", (string message) => app.Logger.LogInformation(message));
app.MapGet(
    "/error",
    (string message) => {
        throw new InvalidOperationException(message);
    }
);

app.Run();
