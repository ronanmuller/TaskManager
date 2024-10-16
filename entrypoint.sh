#!/bin/bash
set -e

# Exibir uma mensagem informativa
echo "Aplicando migrações do banco de dados..."

# Aplicar migrações
dotnet ef database update

# Exibir uma mensagem informativa antes de iniciar a aplicação
echo "Iniciando a aplicação..."

# Iniciar a aplicação
exec dotnet TaskManager.Api.dll
