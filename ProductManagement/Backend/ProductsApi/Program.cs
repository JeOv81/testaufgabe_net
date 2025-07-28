using Application.Features.Products.Commands;
using Application.Interfaces;
using Application.Validations.Products;
using FluentValidation;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using ProductsApi.Endpoints.Products;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;
using Infrastructure.Repositories;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

if (Assembly.GetEntryAssembly()?.GetName().Name != "GetDocument.Insider")
{
    builder.AddServiceDefaults();
}

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularOrigin", 
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                  .AllowAnyHeader()   
                  .AllowAnyMethod()    
                  .AllowCredentials(); // Wichtig wenn Cookies oder Authentifizierungsheader gesendet werden
        });

    options.AddPolicy("AllowBlazorOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:5010", "https://localhost:7193") 
                  .AllowAnyHeader()    
                  .AllowAnyMethod()   
                  .AllowCredentials(); // Wichtig wenn Cookies oder Authentifizierungsheader gesendet werden
        });
});

//Localizer
builder.Services.AddLocalization();

// Optional: Kultur automatisch anhand der Accept-Language setzen
var supportedCultures = new[] { "en", "de" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("en")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});

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

// Scrutor repository registrieren
builder.Services.Scan(scan => scan
    .FromAssemblies(typeof(CategoryRepository).Assembly)
    .AddClasses(classes => classes
        .InNamespaces("Infrastructure.Repositories")
        .Where(type =>
            type.Name.EndsWith("Repository") &&
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

// Auth
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

    builder.Services.AddAuthorization(options =>
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });

    builder.Services.AddAuthorizationBuilder()
        .AddPolicy("admin-policy", policy => policy.RequireRole("admin"));
}
else
{
    // Auth deaktiviert: Leere Policies akzeptieren alles
    builder.Services.AddAuthorization(options =>
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAssertion(_ => true)
            .Build();
    });

    builder.Services.AddAuthorizationBuilder()
        .AddPolicy("admin-policy", policy =>
        {
            policy.RequireAssertion(_ => true);
        });
}


var app = builder.Build();

app.UseRequestLocalization();
app.UseCors("AllowAngularOrigin");
app.UseCors("AllowBlazorOrigin");

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    if (app.Environment.IsDevelopment())
    {
        app.Use(async (context, next) =>
        {
            context.User = new ClaimsPrincipal(new ClaimsIdentity(
                new[]
                {
                new Claim(ClaimTypes.Name, "LocalDevUser"),
                new Claim(ClaimTypes.Role, "admin")
                }, "DevAuth"));

            await next();
        });
    }
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

