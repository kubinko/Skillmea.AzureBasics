var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => $"Hello {Environment.GetEnvironmentVariable("TODAY_NAMESDAY")}!");

app.Run();
