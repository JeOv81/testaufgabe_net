var builder = DistributedApplication.CreateBuilder(args);

var otelCollector = CreateObservabilityContainer(builder);

var sqlserver = builder.AddPostgres("postgresql")
                                                       .WithLifetime(ContainerLifetime.Persistent);
var productsDb = sqlserver.AddDatabase("products-db");

var productsApi = builder.AddProject<Projects.ProductsApi>("products-api")
    .WithEnvironment("OTLP_ENDPOINT_GRPC", "http://localhost:4317")
    .WithEnvironment("SERVICE_NAME", "products-api")
    .WaitFor(otelCollector)
    .WithReference(productsDb)
    .WaitFor(productsDb);

builder.AddProject<Projects.MigrationService>("migrationservice")
    .WithReference(productsDb)
    .WaitFor(productsDb)
    .WithParentRelationship(sqlserver);

builder.AddNpmApp("products-ui-angular", "../../Frontend/ProductsUiAngular")
       .WithReference(productsApi)
       .WithEndpoint(port: 60019, targetPort: 60019, scheme: "http", isProxied: false)
       .WaitFor(productsApi)
       .WithParentRelationship(productsApi);

builder.AddProject<Projects.ProductsUiBlazor>("products-ui-blazor")
    .WithReference(productsApi)
    .WaitFor(productsApi)
    .WithParentRelationship(productsApi);

builder.Build().Run();

static IResourceBuilder<ContainerResource> CreateObservabilityContainer(IDistributedApplicationBuilder builder)
{
    // Grafana
    var grafana = builder.AddContainer("grafana", "grafana/grafana")
        .WithBindMount(source: "./Observability/grafana/config/provisioning", target: "/etc/grafana/provisioning")
        .WithVolume("grafana-data", "/var/lib/grafana")
        .WithHttpEndpoint(3000, targetPort: 3000, name: "grafana");

    // Prometheus
    var prometheus = builder.AddContainer("prometheus", "prom/prometheus")
        .WithBindMount("Observability/prometheus/prometheus-config.yml", "/etc/prometheus/prometheus.yml", isReadOnly: true)
        .WithVolume("prometheus-data", "/prometheus")
        .WithHttpEndpoint(9090, targetPort: 9090, name: "prometheus")
        .WithParentRelationship(grafana);

    // Loki
    var loki = builder.AddContainer("loki", "grafana/loki")
        .WithBindMount("Observability/loki/loki-config.yml", "/etc/loki/local-config.yaml")
        .WithEndpoint(9096, targetPort: 9096, name: "loki")
        .WithHttpEndpoint(3100, targetPort: 3100, name: "loki-http")
        .WithParentRelationship(grafana);

    // Jaeger
    var jaeger = builder.AddContainer("jaeger", "jaegertracing/all-in-one")
        .WithEndpoint(targetPort: 4317, name: "grpc", scheme: "grpc")//neu
        .WithHttpEndpoint(16686, targetPort: 16686, name: "jaeger-ui")
        .WithEndpoint(14250, targetPort: 14250, name: "jaeger")
        .WithParentRelationship(grafana);

    // Tempo
    var tempo = builder.AddContainer("tempo", "grafana/tempo")
        .WithBindMount("Observability/tempo/tempo-config.yml", "/etc/tempo.yaml")
        .WithVolume("tempo-data", "/var/tempo/traces")
        .WithArgs("-config.file=/etc/tempo.yaml")
        .WithEndpoint(targetPort: 4317, name: "grpc", scheme: "grpc")//neu
        .WithHttpEndpoint(3200, targetPort: 3200, name: "tempo-http")
        .WithEndpoint(9095, targetPort: 9095, name: "tempo")
        .WithParentRelationship(grafana);


    // OpenTelemetry Collector
    var otelCollector = builder.AddContainer("otel-collector", "otel/opentelemetry-collector-contrib")
        .WithBindMount("./Observability/otelcol/otel-config.yml", "/etc/otel-contrib/config.yaml")
        .WithEnvironment("SERVICE_NAME", "otel-collector")
        .WithArgs("--config=/etc/otel-contrib/config.yaml")
        .WithEndpoint(4317, targetPort: 4317, name: "otlp-grpc")
        .WithHttpEndpoint(4318, targetPort: 4318, name: "otlp-http")
        .WithHttpEndpoint(targetPort: 9090, name: "prometheus-metrics")//neu
        .WaitFor(jaeger)
        .WaitFor(loki)
        .WaitFor(prometheus)
        .WaitFor(tempo)
        .WithParentRelationship(grafana);
    return otelCollector;
}