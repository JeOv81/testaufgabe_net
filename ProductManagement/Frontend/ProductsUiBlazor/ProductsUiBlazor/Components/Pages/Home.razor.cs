using Microsoft.AspNetCore.Components;
using ProductManagement.ProductsApiClient;
using ProductManagement.ProductsApiClient.Models;

namespace ProductsUiBlazor.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject]
    public required ProductsClient ProductsClient { get; set; }

    public List<ProductDto> ProductList { get; set; } = new List<ProductDto>();
    public List<CategoryDto> CategoryList { get; set; } = new List<CategoryDto>();

    public Home()
    {

    }

    protected override async Task OnInitializedAsync()
    {
        var products = await ProductsClient.Products.GetAsync();
        if(products is not null && products.Any())
        { 
            ProductList.AddRange(products);
        }

        var categories = await ProductsClient.Categories.GetAsync();
        if (categories is not null && categories.Any())
        {
            CategoryList.AddRange(categories);
        }
        await base.OnInitializedAsync();
    }
}
