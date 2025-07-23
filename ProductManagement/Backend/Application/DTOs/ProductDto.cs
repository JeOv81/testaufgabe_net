namespace Application.DTOs;
public record ProductDto(
    Guid Id,
    string Name,
    decimal Price,
    string? Description,
    ICollection<CategoryDto> Categories // Auch die zugehörigen Kategorien als DTOs
);