using System.Security.Claims;

namespace TaskManager.Api.Middleware;

public class RoleSimulationMiddleware
{
    private readonly RequestDelegate _next;

    public RoleSimulationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Simulando o usuário com a role "Manager"
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "TestUser"), // Nome do usuário fictício
            new Claim(ClaimTypes.Role, "Manager") // Atribuindo a role "Manager"
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);

        context.User = principal; // Definindo o usuário no contexto da requisição

        await _next(context); 
    }
}