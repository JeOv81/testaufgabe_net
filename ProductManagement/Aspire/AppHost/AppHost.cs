var builder = DistributedApplication.CreateBuilder(args);

var sqlserver = builder.AddPostgres("postgresql")
                                                       .WithLifetime(ContainerLifetime.Persistent);
var productsDb = sqlserver.AddDatabase("products-db");

builder.AddProject<Projects.ProductsApi>("products-api");

builder.Build().Run();
