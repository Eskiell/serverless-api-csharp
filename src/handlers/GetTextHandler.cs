using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using ServerlessApi.Utils;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ServerlessApi.Handlers;

public class GetTextHandler
{
    public async Task<APIGatewayHttpApiV2ProxyResponse> Handle(
        APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        return await Middleware.WithObservability("getText", request, context,
            (req, ctx) => Task.FromResult(Response.Build(200, new
            {
                message = "Bem-vindo à API Serverless!"
            })));
    }
}
