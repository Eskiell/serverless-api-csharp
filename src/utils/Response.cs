using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;

namespace ServerlessApi.Utils;

public static class Response
{
    public static APIGatewayHttpApiV2ProxyResponse Build(int statusCode, object body)
    {
        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = statusCode,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Access-Control-Allow-Origin", "*" },
                { "Cache-Control", "public, max-age=10" }
            },
            Body = JsonSerializer.Serialize(body)
        };
    }
}
