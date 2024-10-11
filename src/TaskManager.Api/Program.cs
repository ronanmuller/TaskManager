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

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

Configure(app);

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Adiciona controladores, documenta��o e valida��o
    services.AddControllers(options =>
    {
      
    })
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // Configura o MediatR
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProjectCommand).Assembly));

    // Obt�m a connection string do ambiente
    var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                           ?? throw new KeyNotFoundException("Connection string n�o informada.");

    // Configura os DbContexts
    services.AddDbContext<ReadContext>(options => options.UseSqlServer(connectionString));
    services.AddDbContext<WriteContext>(options => options.UseSqlServer(connectionString));

    // Configura AutoMapper
    services.AddAutoMapper(typeof(MappingProfile));

    // Registra reposit�rios
    RegisterRepositories(services);

    // Registra servi�os
    RegisterServices(services);

    // Registra o Accessor de HttpContext
    services.AddHttpContextAccessor();

    // Configura Health Checks
    services.AddHealthChecks()
            .AddDbContextCheck<ReadContext>()
            .AddDbContextCheck<WriteContext>();

    // Configura a autoriza��o
    services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Manager"));
    });
}

// M�todo para registrar reposit�rios
void RegisterRepositories(IServiceCollection services)
{
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddScoped<IProjectRepository, ProjectRepository>();
    services.AddScoped<ITaskRepository, TaskRepository>();
    services.AddScoped<ITaskUpdateHistoryRepository, TaskUpdateHistoryRepository>();
    services.AddScoped<IReportRepository, ReportRepository>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
}

// M�todo para registrar servi�os
void RegisterServices(IServiceCollection services)
{
    services.AddScoped<IProjectService, ProjectService>();
    services.AddScoped<ITaskService, TaskService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IReportService, ReportService>();
}

// M�todo para configurar o pipeline da aplica��o
void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    // Adiciona o middleware para simular roles antes da autoriza��o
    app.UseMiddleware<RoleSimulationMiddleware>();

    app.UseAuthorization();
    app.UseMiddleware<ExceptionMiddleware>(); // Middleware para tratamento de exce��es

    app.MapControllers();
    app.MapHealthChecks("/health");
    app.Run();
}
