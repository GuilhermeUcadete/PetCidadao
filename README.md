# Pet Cidadão

## Sobre o projeto

O Pet Cidadão é uma solução tecnológica voltada à proteção e ao bem-estar animal. O projeto busca facilitar o registro de animais, a divulgação de ocorrências de animais perdidos ou encontrados, denúncias e processos de adoção.

Este repositório contém o desenvolvimento do backend em C# utilizando ASP.NET Core Web API.

## Funcionalidades

- Cadastro e consulta de usuários.
- Cadastro e consulta de animais.
- Registro e consulta de ocorrências de animais perdidos ou encontrados.
- Atualização do status das ocorrências.
- Cadastro e consulta de animais disponíveis para adoção.
- Solicitação de adoção.
- Atualização do status das solicitações de adoção.
- Registro e consulta de denúncias.
- Correspondência entre ocorrências de animais perdidos e encontrados.

## Tecnologias utilizadas

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- LINQ
- Swagger / OpenAPI
- xUnit

## Estrutura do projeto

```text
PetCidadao/
├── PetCidadao.API/
│   ├── Controllers/
│   ├── Data/
│   ├── DTOs/
│   ├── Interfaces/
│   ├── Migrations/
│   ├── Models/
│   ├── Services/
│   ├── Program.cs
│   └── appsettings.json
│
├── PetCidadao.Tests/
│
├── PetCidadao.slnx
└── README.md
```

## Pré-requisitos

Para executar o projeto, é necessário ter instalado:

- .NET SDK 10.
- SQL Server Express.
- Visual Studio Code ou outra IDE compatível com .NET.
- Git.
- Ferramenta Entity Framework Core CLI (`dotnet-ef`).

## Configuração do banco de dados

O projeto utiliza SQL Server com autenticação integrada do Windows.

No arquivo `PetCidadao.API/appsettings.json`, configure a connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=PetCidadaoDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Caso necessário, altere o nome do servidor de acordo com a instalação local do SQL Server.

## Como executar o projeto

### 1. Clonar o repositório

```bash
git clone https://github.com/GuilhermeUcadete/PetCidadao.git
```

### 2. Entrar na pasta do projeto

```bash
cd PetCidadao
```

### 3. Restaurar as dependências

```bash
dotnet restore
```

### 4. Configurar a conexão com o banco de dados

Abra o arquivo `PetCidadao.API/appsettings.json` e configure a connection string conforme as informações do seu SQL Server.

Certifique-se de que a instância do SQL Server esteja instalada e em execução.

### 5. Instalar a ferramenta Entity Framework Core CLI

Caso ainda não tenha a ferramenta instalada, execute:

```bash
dotnet tool install --global dotnet-ef
```

### 6. Entrar na pasta da API

```bash
cd PetCidadao.API
```

### 7. Aplicar as migrations

```bash
dotnet ef database update
```

Esse comando cria ou atualiza o banco de dados conforme as migrations existentes no projeto.

### 8. Executar a aplicação

```bash
dotnet run
```

## Documentação da API

A API possui documentação por meio do Swagger/OpenAPI.

Após iniciar a aplicação, acesse o endereço `/swagger` utilizando a URL local informada no terminal.

A documentação permite visualizar e testar os endpoints disponíveis.

## Testes

O projeto possui testes unitários utilizando xUnit.

Para executar os testes, volte à pasta raiz do projeto e execute:

```bash
cd ..
dotnet test
```

## Integrantes

- Eduardo da Silva Lima (RM: 554804)
- Enzo Bonacasatta (RM: 555372)
- Guilherme Ulacco (RM: 558418)
- Matheus Hostim (RM: 556517)
- Estevam Melo (RM: 555124)
