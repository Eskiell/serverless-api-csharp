using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using ServerlessApi.Utils;

namespace ServerlessApi.Handlers;

public class GetTimeHandler
{
    public async Task<APIGatewayHttpApiV2ProxyResponse> Handle(
        APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        return await Middleware.WithObservability("getTime", request, context,
            (req, ctx) =>
            {
                var now = DateTime.UtcNow;
                return Task.FromResult(Response.Build(200, new
                {
                    horario = now.ToString("o"),
                    timestamp = new DateTimeOffset(now).ToUnixTimeMilliseconds()
                }));
            });
    }
}
