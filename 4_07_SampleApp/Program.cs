var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => $"Good morning {Environment.GetEnvironmentVariable("TODAY_NAMESDAY")} ({Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")})!");

app.Run();
