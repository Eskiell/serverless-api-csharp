# Arquitetura — Serverless API

O diagrama mostra o fluxo completo: **Client → API Gateway** (com as 2 rotas) → **Lambdas** (getText e getTime) → os 3 pilares de observabilidade (**CloudWatch Logs**, **CloudWatch Metrics** via EMF, e **X-Ray tracing**), e na parte inferior o pipeline de deploy (**Serverless Framework → CloudFormation → S3**).

```mermaid
flowchart TB
    Client([Client])

    subgraph AWS["AWS Cloud (us-east-1)"]

        subgraph APIGW["API Gateway (HTTP API)"]
            route1["GET /hello"]
            route2["GET /server-time"]
        end

        subgraph Lambdas["Lambda Functions"]
            getText["getText"]
            getTime["getTime"]
        end

        subgraph Observability["Observabilidade"]
            CWLogs["CloudWatch Logs"]
            CWMetrics["CloudWatch Metrics (EMF)"]
            XRay["AWS X-Ray"]
        end

        subgraph Deploy["Pipeline de Deploy"]
            SLS["Serverless Framework"]
            CFN["CloudFormation"]
            S3["S3 (Artifacts)"]
        end
    end

    Client -->|request| APIGW
    route1 --> getText
    route2 --> getTime

    getText --> CWLogs
    getText --> CWMetrics
    getText --> XRay

    getTime --> CWLogs
    getTime --> CWMetrics
    getTime --> XRay

    SLS --> CFN --> S3
    CFN -.->|provisiona| APIGW
    CFN -.->|provisiona| Lambdas
```
