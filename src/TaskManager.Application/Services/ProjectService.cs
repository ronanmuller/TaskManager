using Microsoft.Extensions.Logging;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Application.Services.Interfaces;
using AutoMapper;
using TaskManager.Application.Dto;

namespace TaskManager.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<ProjectService> _logger;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, ITaskRepository taskRepository, ILogger<ProjectService> logger, IMapper mapper) 
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _logger = logger;
            _mapper = mapper; 
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsByUserIdAsync(int userId)
        {
            _logger.LogInformation("Buscando projetos para o usuário de ID {userId}.", userId);

            var projects = await _projectRepository.GetProjectsByUserIdAsync(userId);
            var projectDtos = _mapper.Map<IEnumerable<ProjectDto>>(projects); 

            return projectDtos;
        }

        public async Task<ProjectDto> CreateProjectAsync(ProjectDto projectDto)
        {
            _logger.LogInformation("Criando novo projeto: {projectName}", projectDto.Name);

            var project = _mapper.Map<Project>(projectDto); 
            await _projectRepository.AddAsync(project);

            _logger.LogInformation("Projeto {projectName} criado com sucesso.", projectDto.Name);

            var createdProjectDto = _mapper.Map<ProjectDto>(project); 
            return createdProjectDto;
        }

        public async Task DeleteProjectAsync(int projectId)
        {
            _logger.LogInformation("Iniciando a exclusão do projeto de ID {projectId}", projectId);

            if (await _taskRepository.HasPendingTasksAsync(projectId))
            {
                _logger.LogWarning("Tentativa de excluir o projeto {projectId}, mas há tarefas pendentes.", projectId);

                throw new InvalidOperationException
                    ("Não é possível excluir um projeto com tarefas pendentes. Conclua as tarefas pendentes e repita a operação.");
            }

            await _projectRepository.DeleteAsync(projectId);

            _logger.LogInformation("Projeto {projectId} excluído com sucesso.", projectId);
        }
    }
}
