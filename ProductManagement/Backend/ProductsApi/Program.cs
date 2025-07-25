using Application.Features.Products.Commands;
using Application.Interfaces;
using Application.Validations.Products;
using FluentValidation;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ProductsApi.Endpoints.Products;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
string serviceName = builder.Configuration["SERVICE_NAME"] ?? "NoServiceName";

if (Assembly.GetEntryAssembly()?.GetName().Name != "GetDocument.Insider")
{
    builder.AddServiceDefaults();
    builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.SetResourceBuilder(CreateResourceBuilder(builder.Configuration));
                metrics.AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation()
                       .AddMeter("Microsoft.AspNetCore.Hosting")
                       .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                       .AddMeter("System.Net.Http")
                       .AddMeter(serviceName);
                metrics.AddOtlpExporter(otlp =>
                {
                    otlp.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    otlp.Endpoint = new Uri(builder.Configuration[EndPointNames.OTLP_ENDPOINT_GRPC]);
                });
            })
            .WithLogging(logging =>
            {
                logging.SetResourceBuilder(CreateResourceBuilder(builder.Configuration));
                logging.AddOtlpExporter(otlp =>
                {
                    otlp.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    otlp.Endpoint = new Uri(builder.Configuration[EndPointNames.OTLP_ENDPOINT_GRPC]);
                });
            })
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddConsoleExporter();
                tracing.AddOtlpExporter(otlp =>
                {
                    otlp.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    otlp.Endpoint = new Uri(builder.Configuration[EndPointNames.OTLP_ENDPOINT_GRPC]);
                });
            });
}

// Scrutor Handler registrieren
builder.Services.Scan(scan => scan
    .FromAssemblies(typeof(CreateProductCommandHandler).Assembly)
    .AddClasses(classes => classes
        .InNamespaces("Application.Features")
        .Where(type =>
            type.Name.EndsWith("Handler") &&
            !type.IsAbstract && !type.IsGenericTypeDefinition))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// Scrutor Endpoints
builder.Services.Scan(scan => scan
    .FromAssemblies(typeof(GetAllProductsEndpoint).Assembly)
    .AddClasses(classes => classes.AssignableTo<IEndpoint>())
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(CreateProductCommandValidator).Assembly);

// Postgresql
builder.AddNpgsqlDbContext<ProductsContext>(connectionName: "products-db");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var endpoints = scope.ServiceProvider.GetServices<IEndpoint>();
    foreach (var endpoint in endpoints)
    {
        endpoint.Map(app);
    }
}

app.Run();

static ResourceBuilder CreateResourceBuilder(IConfiguration configuration)
{
    return ResourceBuilder.CreateDefault()
        .AddService(configuration["SERVICE_NAME"] ?? "NoServiceName");
}
