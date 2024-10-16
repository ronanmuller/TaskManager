using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Mapping;
using TaskManager.Infrastructure.Context;
using TaskManager.Api.Middleware;
using TaskManager.Application.Services;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Infrastructure.Repositories;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Application.MediatorR.Commands.Projects;
using TaskManager.Infrastructure.Repositories.UnitOfWork;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:5000");
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();


// Aplica as migrations automaticamente ao iniciar
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Obtém o contexto de escrita
        var context = services.GetRequiredService<ReadContext>();
        context.Database.Migrate(); // Aplica as migrations
    }
    catch (Exception ex)
    {
        // Log de erros ou qualquer outra ação que você queira fazer em caso de falha
        Console.WriteLine($"Ocorreu um erro ao migrar o banco de dados: {ex.Message}");
    }
}

Configure(app);

[ExcludeFromCodeCoverage]
void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Adiciona controladores, documentação e validação
    services.AddControllers(options =>
    {
      
    })
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // Configura o MediatR
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProjectCommand).Assembly));

    // Obtém a connection string do ambiente
    var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                           ?? throw new KeyNotFoundException("Connection string não informada.");

    // Configura os DbContexts
    services.AddDbContext<ReadContext>(options => options.UseSqlServer(connectionString));
    services.AddDbContext<WriteContext>(options => options.UseSqlServer(connectionString));

    // Configura AutoMapper
    services.AddAutoMapper(typeof(MappingProfile));

    // Registra repositórios
    RegisterRepositories(services);

    // Registra serviços
    RegisterServices(services);

    // Registra o Accessor de HttpContext
    services.AddHttpContextAccessor();

    // Configura Health Checks
    services.AddHealthChecks()
            .AddDbContextCheck<ReadContext>()
            .AddDbContextCheck<WriteContext>();

    // Configura a autorização
    services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Manager"));
    });

    services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            a =>
            {
                a.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    });
}

// Método para registrar repositórios
[ExcludeFromCodeCoverage]
void RegisterRepositories(IServiceCollection services)
{
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddScoped<IProjectRepository, ProjectRepository>();
    services.AddScoped<ITaskRepository, TaskRepository>();
    services.AddScoped<ITaskUpdateHistoryRepository, TaskUpdateHistoryRepository>();
    services.AddScoped<IReportRepository, ReportRepository>();
    services.AddScoped<ITaskCommentRepository, TaskCommentRepository>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
}

// Método para registrar serviços
[ExcludeFromCodeCoverage]
void RegisterServices(IServiceCollection services)
{
    services.AddScoped<IProjectService, ProjectService>();
    services.AddScoped<ITaskService, TaskService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IReportService, ReportService>();
    services.AddScoped<ITaskCommentService, TaskCommentService>();
}


// Método para configurar o pipeline da aplicação
[ExcludeFromCodeCoverage]
void Configure(WebApplication app)
{
    //if (app.Environment.IsDevelopment())
   // {
        app.UseSwagger();
        app.UseSwaggerUI();
    //}

    //app.UseHttpsRedirection();
    app.UseRouting();

    // Adiciona o middleware para simular roles antes da autorização
    app.UseMiddleware<RoleSimulationMiddleware>();
    app.UseCors("AllowAllOrigins");
    app.UseAuthorization();
    app.UseMiddleware<ExceptionMiddleware>(); // Middleware para tratamento de exceções

    app.MapControllers();
    app.MapHealthChecks("/health");
    app.Run();
}
