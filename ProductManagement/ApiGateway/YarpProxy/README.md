Rate limiting Docs: https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?view=aspnetcore-9.0

| Strategie            | Beschreibung                                                                                 | Geeignet für                                                          |
| -------------------- | -------------------------------------------------------------------------------------------- | --------------------------------------------------------------------- |
| **Fixed Window**     | X Anfragen pro fester Zeitspanne (z. B. 10 Anfragen pro 60 s). Einfach und deterministisch.   |  APIs mit konstanter Last, Webhooks                                    |
| **Sliding Window**   | X Anfragen innerhalb eines gleitenden Fensters (z. B. letzte 60 s). Gleichmäßiger verteilt.   |  Benutzerinteraktion mit kurzen Lastspitzen                            |
| **Token Bucket**     | Tokens werden regelmäßig aufgefüllt. Jeder Request verbraucht ein Token.                     | APIs mit Bursts (z. B. wiederholte Klicks, kurzfristiger Lastanstieg) |
| **Concurrency**      | Beschränkt die **gleichzeitig laufenden Anfragen** – unabhängig von Zeit.                    | Ressourcenintensive Operationen (z. B. Datei-Uploads, DB-Intensives)  |
| **Chained (custom)** | Kombination mehrerer Policies (z. B. IP + Benutzer). Selbst konfigurierbar via Partitioning.  | Multi-Tenant, IP- und Benutzer-Scoping, komplexe Regeln               |

### Examples

# Fixed Window
options.AddFixedWindowLimiter("fixed", o =>
{
    o.PermitLimit = 10;
    o.Window = TimeSpan.FromMinutes(1);
});

# Sliding Window
options.AddSlidingWindowLimiter("sliding", o =>
{
    o.PermitLimit = 10;
    o.Window = TimeSpan.FromMinutes(1);
    o.SegmentsPerWindow = 2;
});

# Token Bucket
options.AddTokenBucketLimiter("token", o =>
{
    o.TokenLimit = 20;
    o.TokensPerPeriod = 5;
    o.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
});

# Concurrency
options.AddConcurrencyLimiter("concurrent", o =>
{
    o.PermitLimit = 3;
    o.QueueLimit = 2;
    o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
});

# Chained (custom)
options.AddFixedWindowLimiter("perIp", _ => new()
{
    PermitLimit = 10,
    Window = TimeSpan.FromMinutes(1)
})
.WithPartitioner(httpContext =>
{
    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new()
    {
        PermitLimit = 10,
        Window = TimeSpan.FromMinutes(1),
    });
});

###
###

Loadbalancing Docs: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/yarp/load-balancing?view=aspnetcore-9.0

| Strategie           | Beschreibung                                                        |
| ------------------- | ------------------------------------------------------------------- |
| `RoundRobin`        | Verteilt Anfragen gleichmäßig im Kreis (Standardverhalten).         |
| `LeastRequests`     | Wählt das Ziel mit den **wenigsten aktiven Anfragen**.              |
| `PowerOfTwoChoices` | Wählt zufällig zwei Ziele, nimmt das mit **geringerer Last**.       |
| `Random`            | Wählt ein Ziel per Zufall (nicht empfohlen für produktive Nutzung). |
| `FirstAlphabetical` | Immer das Ziel mit dem **alphabetisch ersten Namen** (Debugging).   |
| `Custom`            | Eigene Implementierung via C# - ILoadBalancingPolicy                |

### Examples

# RoundRobin
...
"Clusters": {
      "cluster": {
        "Destinations": {
          "LoadBalancingPolicy": "RoundRobin",
          "destination1": {
            "Address": "https+http://products-api"
          }
        }
      }
    },
...

# Custom
public sealed class LastLoadBalancingPolicy : ILoadBalancingPolicy
{
    public string Name => "Last";

    public DestinationState? PickDestination(HttpContext context, ClusterState cluster, IReadOnlyList<DestinationState> availableDestinations)
    {
        return availableDestinations[^1];
    }
}