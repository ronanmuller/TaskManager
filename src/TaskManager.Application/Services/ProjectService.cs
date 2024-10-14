using Microsoft.Extensions.Logging;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Application.Services.Interfaces;
using AutoMapper;
using TaskManager.Application.Dto;

namespace TaskManager.Application.Services
{
    public class ProjectService(
        IProjectRepository projectRepository,
        ITaskRepository taskRepository,
        ILogger<ProjectService> logger,
        IMapper mapper)
        : IProjectService
    {
        public async Task<IEnumerable<ProjectDto>> GetProjectsByUserIdAsync(int userId, int skip, int take)
        {
            logger.LogInformation("Buscando projetos para o usuário de ID {userId}.", userId);
            var projects = await projectRepository.GetProjectsByUserIdAsync(userId, skip, take);
            var projectDtos = mapper.Map<IEnumerable<ProjectDto>>(projects); 

            return projectDtos;
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto)
        {
            logger.LogInformation("Criando novo projeto: {projectName}", createProjectDto.Name);
            var project = mapper.Map<Project>(createProjectDto); 
            await projectRepository.AddAsync(project);
            logger.LogInformation("Projeto {projectName} criado com sucesso.", project.Name);
            var createdProjectDto = mapper.Map<ProjectDto>(project); 

            return createdProjectDto;
        }

        public async Task DeleteProjectAsync(int projectId)
        {
            logger.LogInformation("Iniciando a exclusão do projeto de ID {projectId}", projectId);

            if (projectId <= 0)
            {
                throw new ArgumentException("Project id não pode ser menor ou igual a 0."); 
            }

            if (await taskRepository.HasPendingTasksAsync(projectId))
            {
                logger.LogWarning("Tentativa de excluir o projeto {projectId}, mas há tarefas pendentes.", projectId);
                throw new UnauthorizedAccessException
                    ("Não é possível excluir um projeto com tarefas pendentes. Conclua as tarefas pendentes e repita a operação.");
            }

            await projectRepository.DeleteLogicalProjectsByIdAsync(projectId);

            logger.LogInformation("Projeto {projectId} excluído com sucesso.", projectId);
        }
    }
}
