# Etapa 1: Construir a aplicacao e os testes
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia os arquivos de solucao
COPY *.sln .

# Copia todos os projetos
COPY src/TaskManager.Api/*.csproj ./src/TaskManager.Api/
COPY src/TaskManager.Application/*.csproj ./src/TaskManager.Application/
COPY src/TaskManager.Domain/*.csproj ./src/TaskManager.Domain/
COPY src/TaskManager.Infrastructure/*.csproj ./src/TaskManager.Infrastructure/
COPY tests/TaskManager.Tests/*.csproj ./tests/TaskManager.Tests/

# Restaura as dependencias
RUN dotnet restore

# Copia o restante dos arquivos da aplicacao
COPY . .

# Compila a aplicacao em modo Release
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

# Instala o CLI do Entity Framework para aplicar as migracoes
RUN dotnet tool install --global dotnet-ef

# Adiciona o caminho do dotnet tools para o PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Define a porta que a aplicacao ira escutar
EXPOSE 5000

# Define o comando de entrada para aplicar as migracoes e iniciar a aplicacao
ENTRYPOINT ["/bin/bash", "-c", "dotnet ef database update && dotnet TaskManager.Api.dll"]
