# ForjaDosAneis

## Descrição

ForjaDosAneis é um projeto feito para gerenciar anéis mágicos, incluindo suas propriedades e portadores.

## Tecnologias Utilizadas

- .NET 8.0
- Entity Framework Core
- SQL Server
- Swagger

### Pré-requisitos

- .NET 8.0 SDK
- SQL Server

### Configuração do Banco de Dados

1. Atualize a string de conexão no arquivo `appsettings.json` com as informações do seu servidor SQL Server.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=ForjaDosAneisDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

2. Execute as migrações para criar o banco de dados:

```bash
dotnet ef database update
```

### Executando a Aplicação

1. Restaure as dependências do projeto:

```bash
dotnet restore
```

2. Execute a aplicação:

```bash
dotnet run
```

A API estará disponível em `https://localhost:5001` e o Swagger em `https://localhost:5001/swagger`.

## Endpoints

### Anéis

- `GET /api/Aneis`: Retorna todos os anéis.
- `GET /api/Aneis/{id}`: Retorna um anel específico pelo ID.
- `POST /api/Aneis`: Cria um novo anel.
- `PUT /api/Aneis/{id}`: Atualiza um anel existente.
- `DELETE /api/Aneis/{id}`: Deleta um anel pelo ID.

## Licença

Este projeto está licenciado sob a licença MIT.
