using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
string serviceName = builder.Configuration["SERVICE_NAME"] ?? "NoServiceName";

builder.Services.AddServiceDiscovery();
builder.ConfigureOpenTelemetry(); 

builder.Configuration.AddJsonFile("proxysettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();


string policyName = "fixed";
builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: policyName, options =>
    {
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(12);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    }));

var app = builder.Build();

app.UseRateLimiter();

app.MapReverseProxy()
   .RequireRateLimiting(policyName);

app.Run();
