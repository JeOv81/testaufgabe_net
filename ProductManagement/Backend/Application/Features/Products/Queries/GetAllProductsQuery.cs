namespace Application.Features.Products.Queries;
public record GetAllProductsQuery(string? SearchTerm = null, int PageNumber = 1, int PageSize = 10);