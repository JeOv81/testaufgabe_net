using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting; // Für die Extension-Methoden

public static class AppHostExtensions
{
    public static IResourceBuilder<T> WithOpentelemetry<T>(this IResourceBuilder<T> builder, string serviceName, string url = "http://localhost:4317") where T : IResourceWithEnvironment
    {
        builder.WithEnvironment("OTEL_EXPORTER_OTLP_ENDPOINT", url);
        builder.WithEnvironment("SERVICE_NAME", serviceName);
        return builder;
    }
}