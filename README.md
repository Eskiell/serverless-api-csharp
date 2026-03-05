# Serverless API

API REST construída com **C# (.NET 8)**, **AWS Lambda** e **Serverless Framework v4**.

## Estrutura do Projeto

```
serverless-api-csharp/
├── serverless.yml            # Configuração do Serverless Framework
├── package.json              # Scripts de build e deploy
├── src/
│   ├── ServerlessApi.csproj  # Projeto .NET 8
│   ├── Handlers/
│   │   ├── GetTextHandler.cs # Handler GET /hello
│   │   └── GetTimeHandler.cs # Handler GET /server-time
│   └── Utils/
│       ├── Logger.cs         # Logger estruturado (JSON)
│       ├── Metrics.cs        # CloudWatch EMF metrics
│       ├── Middleware.cs     # Middleware de observabilidade
│       └── Response.cs       # Helper para montar respostas HTTP
└── README.md
```

## Endpoints

| Método | Rota           | Descrição                                          |
|--------|----------------|----------------------------------------------------|
| GET    | `/hello`       | Retorna uma mensagem de boas-vindas                |
| GET    | `/server-time` | Retorna o horário atual do servidor (ISO + timestamp) |

### Exemplos de resposta

**GET /hello**
```json
{
  "message": "Bem-vindo à API Serverless!"
}
```

**GET /server-time**
```json
{
  "horario": "2026-03-05T12:00:00.0000000Z",
  "timestamp": 1772366400000
}
```

## Pré-requisitos

- .NET 8 SDK
- Serverless Framework v4 (`npm install -g serverless`)
- AWS CLI configurado com o profile `serverless-deploy`

## Build

```bash
npm run build       # dotnet publish
npm run package     # gera o zip para deploy
```

## Deploy

```bash
npm run deploy           # build + deploy em dev (us-east-1)
npm run deploy:prod      # build + deploy em prod
```

## Remover stack

```bash
npm run remove
```

## Testar localmente (invoke)

```bash
npm run invoke:text      # invoca getText
npm run invoke:time      # invoca getTime
```

## Logs

```bash
npm run logs:text        # logs em tempo real de getText
npm run logs:time        # logs em tempo real de getTime
```

## Configuração (serverless.yml)

| Propriedade  | Valor                        |
|--------------|------------------------------|
| Runtime      | `dotnet8`                    |
| Região       | `us-east-1` (padrão)        |
| Memória      | 256 MB                       |
| Timeout      | 10 s                         |
| Stage padrão | `dev`                        |
```
