using AutoMapper;
using TaskManager.Application.Dto;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;

namespace TaskManager.Application.Services
{
    public class TaskService(ITaskRepository taskRepository, IProjectRepository projectRepository, IMapper mapper) : ITaskService
    {
        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            // Verifica se o projeto existe
            if (!await projectRepository.ExistsAsync(createTaskDto.ProjectId))
            {
                throw new ArgumentException($"Projeto com ID {createTaskDto.ProjectId} não encontrado.");
            }

            // Verifica o número de tarefas existentes para o projeto
            int taskCount = await taskRepository.CountTasksByProjectIdAsync(createTaskDto.ProjectId);
            if (taskCount >= 20) // Limite de 20 tarefas
            {
                throw new InvalidOperationException($"O projeto com ID {createTaskDto.ProjectId} já possui o limite máximo de 20 tarefas.");
            }

            var task = mapper.Map<Tasks>(createTaskDto);
            await taskRepository.AddAsync(task); 
            return mapper.Map<TaskDto>(task); 
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByProjectIdAsync(int projectId, int skip, int take)
        {
            var tasks = await taskRepository.GetTasksByProjectIdAsync(projectId, skip, take);
            return mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task<TaskDto> UpdateTaskAsync(int taskId, UpdateTaskDto updateTaskDto)
        {
            var task = await taskRepository.GetByIdAsync(taskId);

            if (task == null)
            {
                throw new KeyNotFoundException($"Tarefa com ID {taskId} não encontrada."); 
            }

            // Atualiza as propriedades conforme o DTO
            task.Title = updateTaskDto.Title ?? task.Title;
            task.Description = updateTaskDto.Description ?? task.Description; 
            task.Status = updateTaskDto.Status ?? task.Status; 

            await taskRepository.UpdateAsync(task); 

            return mapper.Map<TaskDto>(task); 
        }
    }
}