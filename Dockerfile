# Etapa 1: Construir a aplicação e os testes
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia os arquivos de solução
COPY *.sln .

# Copia todos os projetos
COPY src/TaskManager.Api/*.csproj ./src/TaskManager.Api/
COPY src/TaskManager.Application/*.csproj ./src/TaskManager.Application/
COPY src/TaskManager.Domain/*.csproj ./src/TaskManager.Domain/
COPY src/TaskManager.Infrastructure/*.csproj ./src/TaskManager.Infrastructure/
COPY tests/TaskManager.Tests/*.csproj ./tests/TaskManager.Tests/

# Restaura as dependências
RUN dotnet restore

# Copia o restante dos arquivos da aplicação
COPY . .

# Compila a aplicação em modo Release
RUN dotnet publish src/TaskManager.Api -c Release -o out

# Etapa 2: Executar os testes
FROM build AS test
WORKDIR /app/tests/TaskManager.Tests
RUN dotnet test --no-build --verbosity normal  # Executa os testes

# Etapa 3: Criar a imagem final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia os arquivos compilados da etapa anterior
COPY --from=build /app/out .

# Define a porta que a aplicação irá escutar
EXPOSE 80

# Define o comando para executar a aplicação
ENTRYPOINT ["dotnet", "TaskManager.Api.dll"]
