# Pontos de Melhoria

Sugestões de evolução para este projeto exemplo.

---

## 1. Testes automatizados

- Adicionar testes unitários com **xUnit** para os handlers e utils
- Adicionar um step de `dotnet test` no pipeline antes do deploy
- Considerar testes de integração com **Amazon.Lambda.TestUtilities**

## 2. Validação e linting

- Adicionar **dotnet format** para formatação consistente
- Configurar **.editorconfig** com regras de estilo
- Adicionar **analyzers** (StyleCop, SonarAnalyzer) para análise estática
- Script de lint e step no CI para validar antes do deploy

## 3. Segurança dos endpoints

- Implementar autenticação (API Key, JWT ou Cognito Authorizer)
- Restringir CORS (`Access-Control-Allow-Origin`) para domínios específicos em prod
- Adicionar rate limiting via API Gateway (throttling)

## 4. Variáveis de ambiente e configuração

- Usar **SSM Parameter Store** ou **Secrets Manager** para configurações sensíveis
- Separar variáveis por stage (`dev`/`prod`) de forma mais granular

## 5. Pipeline CI/CD

- Adicionar step de testes e lint antes do deploy
- Adicionar step de **smoke test** após o deploy (chamar os endpoints e validar resposta)

## 6. Monitoramento e alertas

- Configurar **CloudWatch Alarms** para erros 5xx, latência alta e throttles
- Integrar alertas com Slack ou discord
- Criar um dashboard no CloudWatch com métricas das funções

## 7. Infraestrutura

- Adicionar um **custom domain** (ex: `api.seusite.com`) via Route53 + API Gateway
- Considerar **WAF** (Web Application Firewall) para proteção em prod

## 8. Documentação da API

- Gerar documentação com **Swagger/OpenAPI**
- Disponibilizar uma rota `/docs` ou exportar o spec

## 9. Estrutura de código

- Separar lógica de negócio dos handlers em uma camada de **services**
- Usar **Lambda Layers** ou **NuGet packages** para dependências compartilhadas quando o projeto crescer
- Considerar **Dependency Injection** via `IServiceProvider` para facilitar testes

## 10. Performance

- Considerar **Lambda@Edge** ou **CloudFront** para cache em nível de CDN
