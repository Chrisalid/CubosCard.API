
# CubosCard.API

Este projeto é uma API desenvolvida em .NET para gerenciamento de cartões, contas e transações, utilizando arquitetura modular e Entity Framework Core para persistência de dados.

## Pré-requisitos

- [.NET SDK 6.0 ou superior](https://dotnet.microsoft.com/download)
- Banco de dados SQL Server (ou outro compatível com EF Core)

## Estrutura do Projeto

- `src/CubosCard.Api/` — Projeto principal da API
- `src/CubosCard.Application/` — Camada de aplicação (serviços, DTOs)
- `src/CubosCard.Domain/` — Entidades de domínio, enums, interfaces
- `src/CubosCard.Infrastructure/` — Persistência, repositórios, migrações
- `src/Clients/CubosCard.External.API/` — Integrações externas
- `src/CubosCard.IoC/` — Injeção de dependências e configurações

## Como executar a API

1. **Restaurar dependências**

   ```pwsh
   dotnet restore
   ```

2. **Compilar o projeto**

   ```pwsh
   dotnet build src/CubosCard.Api/CubosCard.Api.csproj
   ```

3. **Executar migrações do banco de dados**

   ```pwsh
   dotnet ef database update --project CubosCard.Api.Infrastructure --startup-project CubosCard.Api
   ```

4. **Executar a API**

   ```pwsh
   dotnet run --project src/CubosCard.Api/CubosCard.Api.csproj
   ```

   A API estará disponível em `https://localhost:5001` ou conforme configurado no `launchSettings.json`.

## Ambiente de Desenvolvimento

- Utilize o arquivo `appsettings.Development.json` para configurar variáveis de ambiente e strings de conexão.
- Para rodar em modo watch (hot reload):

  ```pwsh
  dotnet watch run --project src/CubosCard.Api/CubosCard.Api.csproj
  ```

## Testes

Os testes podem ser adicionados em projetos separados. Para rodar testes, utilize:

```pwsh
dotnet test
```

## Migrações

Para criar uma nova migração:

```pwsh
dotnet ef migrations add NOME_DA_MIGRACAO --project CubosCard.Api.Infrastructure --startup-project CubosCard.Api
```

## Contato

Dúvidas ou sugestões? Entre em contato com o mantenedor do projeto.
