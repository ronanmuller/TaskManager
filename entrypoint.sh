#!/bin/bash
set -e

# Exibir uma mensagem informativa
echo "Aplicando migra��es do banco de dados..."

# Aplicar migra��es
dotnet ef database update

# Exibir uma mensagem informativa antes de iniciar a aplica��o
echo "Iniciando a aplica��o..."

# Iniciar a aplica��o
exec dotnet TaskManager.Api.dll
