using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Scrutor Handler registrieren
builder.Services.Scan(scan => scan
    .FromAssemblies(typeof(Application.Features.Products.Commands.CreateProductCommandHandler).Assembly)
    .AddClasses(classes => classes
        .InNamespaces("Application.Features")
        .Where(type =>
            type.Name.EndsWith("Handler") &&
            !type.IsAbstract && !type.IsGenericTypeDefinition))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// Scrutor Endpoints
builder.Services.Scan(scan => scan
    .FromAssemblies(typeof(ProductsApi.Endpoints.Products.GetAllProductsEndpoint).Assembly) 
    .AddClasses(classes => classes.AssignableTo<IEndpoint>())
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// Postgresql
builder.AddNpgsqlDbContext<ProductsContext>(connectionName: "products-db");

// Secure endpoints without attributes
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddAuthorizationBuilder()
  .AddPolicy("admin-policy", policy =>
        policy.RequireRole("admin"));

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