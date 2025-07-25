namespace Application.Features.Products.Queries;
public record GetAllProductsQuery(string? SearchTerm = null, int PageNumber = 1, int PageSize = 10, string? OrderBy = null, bool? Ascending = null);