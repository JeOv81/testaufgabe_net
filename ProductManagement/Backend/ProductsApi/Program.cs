using Infrastructure.Persistence;

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

app.Run();