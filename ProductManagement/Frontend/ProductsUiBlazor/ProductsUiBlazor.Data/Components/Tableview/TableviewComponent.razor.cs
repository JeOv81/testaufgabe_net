using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace ProductsUiBlazor.Data.Components.Tableview;

public partial class TableviewComponent<TItem> : ComponentBase
{
    private MemberInfo[] memberInfos;

    [Parameter]
    public List<TItem> Items { get; set; } = new();

    [Parameter]
    public List<string> IgnoreMember { get; set; } = new();

    protected override Task OnParametersSetAsync()
    {
        base.OnParametersSetAsync();

        if (Items != null && Items.Count > 0)
        {
            var type = Items[0].GetType();
            memberInfos = type.GetMembers();
        }

        return Task.FromResult(0);
    }
}
