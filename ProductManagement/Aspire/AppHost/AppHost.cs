var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ProductsApi>("products-api");

builder.Build().Run();
