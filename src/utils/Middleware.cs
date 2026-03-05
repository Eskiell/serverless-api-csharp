using System.Diagnostics;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace ServerlessApi.Utils;

public static class Middleware
{
    public static async Task<APIGatewayHttpApiV2ProxyResponse> WithObservability(
        string functionName,
        APIGatewayHttpApiV2ProxyRequest request,
        ILambdaContext context,
        Func<APIGatewayHttpApiV2ProxyRequest, ILambdaContext, Task<APIGatewayHttpApiV2ProxyResponse>> handler)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = context.AwsRequestId;

        Logger.Info("Request received", new Dictionary<string, object?>
        {
            { "functionName", functionName },
            { "requestId", requestId },
            { "path", request.RawPath },
            { "method", request.RequestContext?.Http?.Method ?? "UNKNOWN" },
            { "queryParams", request.QueryStringParameters },
            { "userAgent", request.Headers != null && request.Headers.TryGetValue("user-agent", out var ua) ? ua : null }
        });

        Metrics.RequestCount(functionName);

        try
        {
            var response = await handler(request, context);
            stopwatch.Stop();

            Metrics.Latency(functionName, stopwatch.ElapsedMilliseconds);

            Logger.Info("Request completed", new Dictionary<string, object?>
            {
                { "functionName", functionName },
                { "requestId", requestId },
                { "statusCode", response.StatusCode },
                { "duration", $"{stopwatch.ElapsedMilliseconds}ms" }
            });

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            Metrics.ErrorCount(functionName);
            Metrics.Latency(functionName, stopwatch.ElapsedMilliseconds);

            Logger.Error("Request failed", new Dictionary<string, object?>
            {
                { "functionName", functionName },
                { "requestId", requestId },
                { "error", ex.Message },
                { "stack", ex.StackTrace },
                { "duration", $"{stopwatch.ElapsedMilliseconds}ms" }
            });

            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 500,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" }
                },
                Body = JsonSerializer.Serialize(new { message = "Erro interno do servidor" })
            };
        }
    }
}
