using System.Text.Json;

namespace ServerlessApi.Utils;

// CloudWatch Embedded Metric Format (EMF)
// Publica métricas customizadas sem custo extra via logs estruturados
public static class Metrics
{
    private static void PutMetric(string ns, string metricName, double value, string unit, Dictionary<string, string> dimensions)
    {
        var emf = new Dictionary<string, object>
        {
            {
                "_aws", new Dictionary<string, object>
                {
                    { "Timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() },
                    {
                        "CloudWatchMetrics", new[]
                        {
                            new Dictionary<string, object>
                            {
                                { "Namespace", ns },
                                { "Dimensions", new[] { dimensions.Keys.ToArray() } },
                                { "Metrics", new[] { new Dictionary<string, string> { { "Name", metricName }, { "Unit", unit } } } }
                            }
                        }
                    }
                }
            },
            { metricName, value }
        };

        foreach (var kvp in dimensions)
            emf[kvp.Key] = kvp.Value;

        Console.WriteLine(JsonSerializer.Serialize(emf));
    }

    private static Dictionary<string, string> DefaultDimensions(string functionName) => new()
    {
        { "FunctionName", functionName },
        { "Stage", Environment.GetEnvironmentVariable("STAGE") ?? "unknown" }
    };

    public static void RequestCount(string functionName) =>
        PutMetric("ServerlessAPI", "RequestCount", 1, "Count", DefaultDimensions(functionName));

    public static void Latency(string functionName, double durationMs) =>
        PutMetric("ServerlessAPI", "Latency", durationMs, "Milliseconds", DefaultDimensions(functionName));

    public static void ErrorCount(string functionName) =>
        PutMetric("ServerlessAPI", "ErrorCount", 1, "Count", DefaultDimensions(functionName));
}
