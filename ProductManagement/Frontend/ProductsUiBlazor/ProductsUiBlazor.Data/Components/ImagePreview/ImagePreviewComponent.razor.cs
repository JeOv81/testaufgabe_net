using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Msoft.Dms.Web.SharedComponents.Resources.UI;

namespace ProductsUiBlazor.Data.Components.ImagePreview;

public partial class ImagePreviewComponent : ComponentBase
{
    [Parameter]
    public RenderFragment Toolbar { get; set; }

    [Parameter]
    public bool? Loading { get; set; }

    [Parameter]
    public string Filename { get; set; }

    [Parameter]
    public string ImageUrl { get; set; }

    [Inject]
    public IStringLocalizer<Resource> Localizer { get; set; }

    protected override Task OnParametersSetAsync()
    {
        return base.OnParametersSetAsync();
    }
}
