using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using NUnit.Framework;
using ProductManagement.ProductsApiClient;
using ProductManagement.ProductsApiClient.Models;
using Shouldly;
using System.Net;
using Testcontainers.PostgreSql;


namespace Tests.Tests;

[TestFixture]
public class ApiTests
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60);
    private ProductsClient _client = null!;
    private Guid _categoryId;
    private Guid _productId;

    private IDistributedApplicationTestingBuilder _appHostBuilder = null!; 
    private DistributedApplication _app = null!;
    private PostgreSqlContainer _dedicatedTestPostgresContainer = null!;
    private List<CategoryDto>? _categories;
    private List<ProductDto>? _products;

    [OneTimeSetUp]
    public async Task OneTimeSetUpAsync()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));

        _appHostBuilder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>();
        _appHostBuilder.Services.AddServiceDiscovery();
        _appHostBuilder.Services.ConfigureHttpClientDefaults(http =>
        {
            http.AddServiceDiscovery();
        });

        var postgresql = _appHostBuilder.CreateResourceBuilder((PostgresServerResource)_appHostBuilder.Resources.First(r => r.Name == "postgresql"));
        postgresql.WithLifetime(ContainerLifetime.Session);
        var testdb = postgresql.AddDatabase("testdb");
        
        var migrationservice = _appHostBuilder.CreateResourceBuilder((ProjectResource)_appHostBuilder.Resources.First(r => r.Name == "migrationservice"));
        migrationservice.WithReference(testdb);
        migrationservice.WaitFor(testdb);

        var products_api = _appHostBuilder.CreateResourceBuilder((ProjectResource)_appHostBuilder.Resources.First(r => r.Name == "products-api"));
        products_api.WithReference(testdb);
        products_api.WaitFor(testdb);

        _app = await _appHostBuilder.BuildAsync().WaitAsync(DefaultTimeout, cts.Token);
        await _app.StartAsync().WaitAsync(DefaultTimeout, cts.Token);

        var resultState = await _app.ResourceNotifications.WaitForResourceHealthyAsync("products-api", cts.Token);

        var httpclient = _app.CreateHttpClient("products-api");
        Console.WriteLine("BaseAddress resolved to: " + httpclient.BaseAddress);
        var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: httpclient);
        _client = new ProductsClient(adapter);
    }

    [SetUp]
    public async Task SetUpAsync()
    {
        _categories = await _client.Categories.GetAsync();
        _categories.ShouldNotBeNull();
        _categories.Count.ShouldBeGreaterThanOrEqualTo(3);
        _products = await _client.Products.GetAsync();
        _products.ShouldNotBeNull();
        _products.Count.ShouldBeGreaterThanOrEqualTo(4);
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _categories = null;
        _products = null;
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDownAsync()
    {
        await _app.StopAsync().WaitAsync(DefaultTimeout);
        _app.Dispose();
        _appHostBuilder.Dispose(); 

        if (_dedicatedTestPostgresContainer != null)
        {
            await _dedicatedTestPostgresContainer.StopAsync();
            await _dedicatedTestPostgresContainer.DisposeAsync();
        }
    }

    [Test]
    [Order(1)]
    public async Task CreateProduct_ShouldSucceed()
    {
        var newProduct = new CreateProductCommand
        {
            Name = "KiotaAspireProduct",
            Description = "Created via Aspire Kiota",
            Price = 42.42,
            CategoryIds = [_categories?.First().Id]
        };

        var productId = await _client.Products.PostAsync(newProduct);
        productId.ShouldNotBeNull();
        _productId = productId!.Value;
        _productId.ShouldNotBe(Guid.Empty);

        var product = await _client.Products[_productId].GetAsync();
        product.ShouldNotBeNull();
        product!.Name.ShouldBe("KiotaAspireProduct");
    }

    [Test]
    [Order(2)]
    public async Task GetProductById_ShouldReturnProduct()
    {
        var product = await _client.Products[_productId].GetAsync();

        product.ShouldNotBeNull();
        product!.Id.ShouldBe(_productId);
        product.Name.ShouldBe("KiotaAspireProduct");
    }

    [Test]
    [Order(3)]
    public async Task UpdateProduct_ShouldSucceed()
    {
        var updateRequest = new UpdateProductCommand
        {
            Id = _productId,
            Name = "UpdatedAspireProduct",
            Description = "Updated via Aspire Kiota",
            Price = 99.99,
            CategoryIds = [_categories?.First().Id]
        };

        await _client.Products.PutAsync(updateRequest);

        var updatedProduct = await _client.Products[_productId].GetAsync();
        updatedProduct.ShouldNotBeNull();
        updatedProduct!.Name.ShouldBe("UpdatedAspireProduct");
        updatedProduct.Price.ShouldBe(99.99);
    }

    [Test]
    [Order(4)]
    public async Task DeleteProduct_ShouldSucceedAndReturnNotFound()
    {
        var id = _products?.First().Id;
        var deleteProductCommand = new DeleteProductCommand { Id = id };
        await _client.Products.DeleteAsync(deleteProductCommand);

        var ex = await Should.ThrowAsync<Microsoft.Kiota.Abstractions.ApiException>(async () =>
        {
            await _client.Products[id!.Value].GetAsync();
        });

        ex.ResponseStatusCode.ShouldBe((int)HttpStatusCode.NotFound);
    }
}