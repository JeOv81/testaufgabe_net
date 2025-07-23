var builder = DistributedApplication.CreateBuilder(args);

var sqlserver = builder.AddPostgres("postgresql")
                                                       .WithLifetime(ContainerLifetime.Persistent);
var productsDb = sqlserver.AddDatabase("products-db");

var productsApi = builder.AddProject<Projects.ProductsApi>("products-api")
    .WithReference(productsDb)
    .WaitFor(productsDb);

builder.Build().Run();
