# Projeto Template API .NET 8

Este é um template para um projeto API utilizando .NET 8, Entity Framework, AutoMapper, padrão Unit of Work e Result Pattern. Este template pode ser usado como base para futuras implementações de APIs em projetos .NET.

## Estrutura do Projeto

- **API**: Camada de apresentação que expõe os endpoints HTTP.
- **Aplicação**: Camada de aplicação que contém os serviços de aplicação.
- **Domínio**: Camada que contém as entidades e regras de negócio.
- **Infraestrutura**: Camada que contém a implementação de persistência de dados e outros serviços de infraestrutura.

## Tecnologias Utilizadas

- .NET 8
- Entity Framework Core
- AutoMapper
- Padrão Unit of Work
- Result Pattern

## Configuração do Projeto

### Pré-requisitos

- .NET 8 SDK
- SQL Server ou outro banco de dados compatível com Entity Framework

### Instalação

1. Clone o repositório:
    ```bash
    git clone https://github.com/usuario/projeto-template-api.git
    cd projeto-template-api
    ```

2. Restaure os pacotes NuGet:
    ```bash
    dotnet restore
    ```

3. Configure a string de conexão no arquivo `appsettings.json`:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=seu_servidor;Database=sua_base_de_dados;User Id=seu_usuario;Password=sua_senha;"
      }
    }
    ```

4. Execute as migrações para configurar o banco de dados:
    ```bash
    dotnet ef database update
    ```

5. Execute o projeto:
    ```bash
    dotnet run
    ```

## Estrutura do Código

### Unit of Work

O padrão Unit of Work é implementado para gerenciar a transação de múltiplos repositórios. Isso garante que todas as operações de banco de dados em uma determinada unidade de trabalho sejam tratadas como uma única transação.

### Result Pattern

O Result Pattern é utilizado para retornar o status das operações. Isso ajuda a lidar com os diferentes resultados de uma operação (sucesso, falha, etc.) de uma maneira consistente.

#### Classe Result

```csharp
using System.Net;

namespace Template.Domain;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public ErrorDetails? Error { get; set; }
    public T? Data { get; set; }
    public PaginationDetails? Pagination { get; set; }

    public static Result<T> Success(T data)
    {
        return new Result<T> { IsSuccess = true, Data = data };
    }

    public static Result<T> Success(T data, int pageSize, int pageNumber, int totalCount)
    {
        return new Result<T> { IsSuccess = true, Data = data, Pagination = new PaginationDetails(pageSize, pageNumber, totalCount) };
    }

    public static Result<T> Failure(HttpStatusCode statusCode, string errorMessage)
    {
        return new Result<T> { IsSuccess = false, Error = new ErrorDetails(statusCode, errorMessage) };
    }
}

public class ErrorDetails
{
    public ErrorDetails(HttpStatusCode statusCode, string message)
    {
        StatusCode = (int)statusCode;
        Message = message;
    }
    public int StatusCode { get; private set; }
    public string Message { get; private set; }
}

public class PaginationDetails : PaginationParams
{
    public PaginationDetails(int pageSize, int pageNumber, int totalCount)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
    public int TotalCount { get; set; }
}
