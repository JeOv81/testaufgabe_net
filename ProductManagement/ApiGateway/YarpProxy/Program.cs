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


var app = builder.Build();

app.MapReverseProxy();

app.Run();
