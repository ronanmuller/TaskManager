FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln .

COPY src/TaskManager.Api/*.csproj ./src/TaskManager.Api/
COPY src/TaskManager.Application/*.csproj ./src/TaskManager.Application/
COPY src/TaskManager.Domain/*.csproj ./src/TaskManager.Domain/
COPY src/TaskManager.Infrastructure/*.csproj ./src/TaskManager.Infrastructure/
COPY tests/TaskManager.Tests/*.csproj ./tests/TaskManager.Tests/

COPY src/TaskManager.Api/appsettings.json ./src/TaskManager.Api/

RUN dotnet restore

COPY . .

RUN dotnet publish src/TaskManager.Api -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 5000

CMD ["dotnet", "TaskManager.Api.dll"]
