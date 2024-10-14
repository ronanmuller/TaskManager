using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application.Services;

[ExcludeFromCodeCoverage]
public class UserService : IUserService
{
    private readonly Random _random = new Random();
    private const int MaxUserId = 100; // Defina o número máximo de usuários existentes

    public int GetCurrentUserId()
    {
        return GetRandomUserId();
    }

    private int GetRandomUserId()
    {
        // Gera um número aleatório entre 1 e MaxUserId
        return _random.Next(1, MaxUserId + 1);
    }
}