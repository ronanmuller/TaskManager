# Task Manager

Este projeto é uma aplicação de gerenciamento de tarefas desenvolvida em .NET. A aplicação permite criar, atualizar, listar e gerenciar tarefas e projetos, além de definir prioridades para cada tarefa.

## Índice

- [Requisitos](#requisitos)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Configuração do Ambiente](#configuração-do-ambiente)
- [Executando a Aplicação com Docker](#executando-a-aplicação-com-docker)
- [Parar a Aplicação](#parar-a-aplicação)
- [Estrutura do Banco de Dados](#estrutura-do-banco-de-dados)
- [Executando Testes](#executando-testes)
- [Contribuição](#contribuição)
- [Licença](#licença)

## Requisitos

Para executar esta aplicação, você precisará dos seguintes softwares instalados:

- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/)


-
## Estrutura do Projeto
src/ 
├── TaskManager.Api/ # Projeto da API
├── TaskManager.Application/ # Camada de aplicação 
├── TaskManager.Domain/ # Domínio da aplicação 
└── TaskManager.Infrastructure/ 
# Infraestrutura tests/ 
tests/
└── TaskManager.Tests/ # Projetos de testes



## Configuração do Ambiente

### Variáveis de Ambiente

As seguintes variáveis de ambiente são necessárias para a configuração da aplicação:

- `ASPNETCORE_ENVIRONMENT`: Define o ambiente da aplicação. O valor padrão é `Development`.
- `ConnectionStrings__DefaultConnection`: String de conexão do banco de dados. 
- Exemplo:
Server=db;Database=TaskManagerDB;User Id=sa;Password=YourPassword123;TrustServerCertificate=True;



## Passo a Passo para Rodar a Aplicação no Docker Desktop

### 1. Pré-requisitos
- **Docker Desktop**: Certifique-se de que o Docker Desktop está instalado e em execução em sua máquina. Você pode baixar a versão mais recente do Docker [aqui](https://www.docker.com/products/docker-desktop).

### 2. Clonar o Repositório
- Clone o repositório da aplicação para sua máquina local:
  ```bash
  git clone https://github.com/ronanmuller/TaskManager.git

 
### 3. Navegue até o diretório do projeto
Exemplo: cd TaskManager



### 4. Construir a Imagem do Docker
docker-compose build	


### 5. Executar o Docker Compose
docker-compose up



## Parar a Aplicação

Para parar a execução da aplicação e remover os containers Docker, siga os passos abaixo:

1. **Parar os containers**:
   Execute o comando para parar a aplicação e os serviços relacionados:
   ```bash
   docker-compose down



## Estrutura do Banco de Dados

A aplicação utiliza o SQL Server como banco de dados. As tabelas principais são:

- **Projects**: Tabela que armazena informações sobre os projetos.
  - `ProjectId` (int, chave primária)
  - `Name` (nvarchar, tamanho variável)
  - `UserId` (int, identifica o usuário dono do projeto)
  - `Status` (int, representando o estado do projeto)
  - `CreatedAt` (datetime)

- **Tasks**: Tabela que armazena as tarefas associadas aos projetos.
  - `TaskId` (int, chave primária)
  - `Title` (nvarchar, tamanho variável)
  - `Priority` (int, enum para baixa, média, alta)
  - `Status` (int, enum para pendente, concluída, cancelada)
  - `ProjectId` (int, chave estrangeira para Projects)
  - `CreatedAt` (datetime)

- **ProjectTasks**: Tabela que faz a associação entre projetos e tarefas.
  - `ProjectId` (int, chave estrangeira)
  - `TaskId` (int, chave estrangeira)

O banco de dados é configurado automaticamente via Entity Framework migrations ao rodar o comando `dotnet ef database update` durante a execução do Docker Compose.


Acessar a Aplicação
http://localhost:5000/swagger


## Executando Testes

Para executar os testes da aplicação, siga os passos abaixo:

1. Certifique-se de que a aplicação foi construída corretamente. Caso contrário, execute:
   ```bash
   dotnet build
   cd tests/TaskManager.Tests
   dotnet test
