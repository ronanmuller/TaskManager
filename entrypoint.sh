#!/bin/bash

# Aplica as migracoes
dotnet ef database update

# Inicia a aplicacao
dotnet TaskManager.Api.dll
