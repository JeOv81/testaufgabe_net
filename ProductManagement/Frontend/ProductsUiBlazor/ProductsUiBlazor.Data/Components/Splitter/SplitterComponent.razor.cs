using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProductsUiBlazor.Data.Enums;

namespace ProductsUiBlazor.Data.Components.Splitter;

public partial class SplitterComponent : ComponentBase, IAsyncDisposable
{
    [Parameter]
    public RenderFragment SplitArea1 { get; set; }

    [Parameter]
    public RenderFragment SplitArea2 { get; set; }

    [Parameter]
    public string StyleArea1 { get; set; } = "";

    [Parameter]
    public string StyleArea2 { get; set; } = "";

    [Parameter]
    public SplitterOrientation Orientation { get; set; } = SplitterOrientation.Horizontal;

    [Parameter]
    public int[] Size { get; set; } = new int[] { 20, 80 };

    [Parameter]
    public int[] MinSize { get; set; } = new int[] { 340 };

    [Inject]
    public IJSRuntime JS { get; set; }

    private IJSObjectReference splitter;

    private bool isRendered;
    private string nameArea1;
    private string nameArea2;

    public string ClassNameArea1 { get; set; }
    public string ClassNameArea2 { get; set; }

    protected override Task OnParametersSetAsync()
    {
        if (string.IsNullOrEmpty(nameArea1))
        {
            nameArea1 = Guid.NewGuid().ToString("N").ToLower();
            ClassNameArea1 = $"split area{nameArea1}";
        }
        if (string.IsNullOrEmpty(nameArea2))
        {
            nameArea2 = Guid.NewGuid().ToString("N").ToLower();
            ClassNameArea2 = $"split area{nameArea2}";
        }

        return base.OnParametersSetAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!isRendered)
        {
            splitter = await JS.InvokeAsync<IJSObjectReference>("Split", new[] { $".area{nameArea1}", $".area{nameArea2}" }, new
            {
                sizes = Size,
                minSize = MinSize,
                direction = Orientation.ToString().ToLower(CultureInfo.CurrentCulture),
            });

            isRendered = true;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (splitter != null)
        {
            GC.SuppressFinalize(this);
            await splitter.InvokeVoidAsync("destroy");
            splitter = null;
        }
    }
}
