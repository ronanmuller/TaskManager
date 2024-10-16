#!/bin/bash

# Remove o banco de dados existente
dotnet ef database drop --force --context ReadContext

# Adiciona uma nova migra��o
dotnet ef migrations add InitialCreate --context ReadContext

# Atualiza o banco de dados com a nova migra��o
dotnet ef database update --context ReadContext

# Lista as migra��es aplicadas
dotnet ef migrations list --context ReadContext

# Inicia a aplica��o
dotnet TaskManager.Api.dll
