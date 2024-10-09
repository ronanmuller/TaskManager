using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Mapping;
using TaskManager.Infrastructure.Context;
using TaskManager.Api.Middleware;
using TaskManager.Application.Services;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Infrastructure.Repositories;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Application.MediatorR.Handlers.Projects;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();
Configure(app);

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddDbContext<ReadContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    services.AddDbContext<WriteContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    services.AddAutoMapper(typeof(MappingProfile));

    // Injeção de dependência dos repositórios
    services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
    services.AddScoped<IProjectRepository, ProjectRepository>();
    services.AddScoped<ITaskRepository, TaskRepository>();

    // Injeção de dependência dos serviços
    services.AddScoped<IProjectService, ProjectService>();
    services.AddScoped<ITaskService, TaskService>();

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProjectCommandHandler).Assembly));

    services.AddHealthChecks()
        .AddDbContextCheck<ReadContext>() // Verifica a saúde do ReadContext
        .AddDbContextCheck<WriteContext>(); // Verifica a saúde do WriteContext
}

void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthorization();
    app.UseMiddleware<ExceptionMiddleware>();

    app.MapControllers();
    app.MapHealthChecks("/health");
    app.Run();
}
