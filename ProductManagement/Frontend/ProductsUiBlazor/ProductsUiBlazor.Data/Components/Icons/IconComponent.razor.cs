using Microsoft.AspNetCore.Components;
using ProductsUiBlazor.Data.Utilities;

namespace ProductsUiBlazor.Data.Components.Icons;

public partial class IconComponent : ComponentBase
{
    [Parameter]
    public string FileExtension { get; set; } = "svg";

    [Parameter]
    public string DefaultBasePath { get; set; } = $"_content/{CurrentNamespace.GetCurrentNamespace()}Web.SharedComponents/icons";

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public bool IdentifyByName { get; set; } = false;

}
