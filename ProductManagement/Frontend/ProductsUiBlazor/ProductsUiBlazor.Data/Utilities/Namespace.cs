using System.Reflection;
using System.Runtime.CompilerServices;

namespace ProductsUiBlazor.Data.Utilities;

public static class CurrentNamespace
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string? GetCurrentNamespace()
    {
        return Assembly.GetCallingAssembly().EntryPoint?.DeclaringType?.Namespace;
    }
}