using Application.Features.Products.Commands;
using Application.Interfaces;
using Application.Validations.Products;
using FluentValidation;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ProductsApi.Endpoints.Products;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

if (Assembly.GetEntryAssembly()?.GetName().Name != "GetDocument.Insider")
{
    builder.AddServiceDefaults();
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

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddAuthorization(options =>
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });

    builder.Services.AddAuthorizationBuilder()
                    .AddPolicy("admin-policy", policy => policy.RequireRole("admin"));

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"], // Aus appsettings.json
                ValidAudience = builder.Configuration["Jwt:Audience"], // Aus appsettings.json
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Aus appsettings.json
            };
        });
}

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

