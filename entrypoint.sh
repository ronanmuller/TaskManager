#!/bin/bash

# Remove o banco de dados existente
dotnet ef database drop --force --context ReadContext

# Adiciona uma nova migração
dotnet ef migrations add InitialCreate --context ReadContext

# Atualiza o banco de dados com a nova migração
dotnet ef database update --context ReadContext

# Lista as migrações aplicadas
dotnet ef migrations list --context ReadContext

# Inicia a aplicação
dotnet TaskManager.Api.dll
