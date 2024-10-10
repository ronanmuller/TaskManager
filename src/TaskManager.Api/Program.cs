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

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();
Configure(app);

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProjectCommand).Assembly));
    services.AddControllers().AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()); });

    string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

    services.AddDbContext<ReadContext>(options =>
        options.UseSqlServer(connectionString));

    services.AddDbContext<WriteContext>(options =>
        options.UseSqlServer(connectionString));

    services.AddAutoMapper(typeof(MappingProfile));

    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

    // Injeção de dependência dos repositórios
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddScoped<IProjectRepository, ProjectRepository>();
    services.AddScoped<ITaskRepository, TaskRepository>();

    // Injeção de dependência dos serviços
    services.AddScoped<IProjectService, ProjectService>();
    services.AddScoped<ITaskService, TaskService>();


    services.AddHealthChecks()
        .AddDbContextCheck<ReadContext>()
        .AddDbContextCheck<WriteContext>();
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
