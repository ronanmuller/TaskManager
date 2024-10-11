using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using TaskManager.Application.Dto;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;

namespace TaskManager.Application.Services
{
    public class TaskService(
        ITaskRepository taskRepository,
        IProjectRepository projectRepository,
        ITaskUpdateHistoryRepository taskUpdateHistoryRepository,
        IUserService userService,
        IMapper mapper, IUnitOfWork unitOfWork)
        : ITaskService
    {
        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            // Verifica se o projeto existe
            if (!await projectRepository.ExistsAsync(createTaskDto.ProjectId))
            {
                throw new ArgumentException($"Projeto com ID {createTaskDto.ProjectId} não encontrado ou finalizado.");
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
            // Estou usando o unit of work aqui para garantir que so teremos um registro de update se ele realmente acontecer
            await unitOfWork.BeginTransactionAsync(); 

            try
            {
                var task = await unitOfWork.Tasks.GetByIdAsync(taskId);

                if (task == null)
                {
                    throw new KeyNotFoundException($"Tarefa com ID {taskId} não encontrada.");
                }

                // Armazena os detalhes da modificação
                var updateDetail = GetUpdateDetails(task, updateTaskDto);

                // Atualiza as propriedades conforme o DTO
                task.Title = updateTaskDto.Title ?? task.Title;
                task.Description = updateTaskDto.Description ?? task.Description;
                task.Status = updateTaskDto.Status ?? task.Status;

                await unitOfWork.Tasks.UpdateAsync(task);

                // Salva o histórico da atualização
                var updateHistory = new TaskUpdateHistory
                {
                    TaskId = task.Id,
                    UserId = userService.GetCurrentUserId(), // ID do usuário que fez a alteração
                    UpdateDate = DateTime.Now,
                    UpdateDetail = updateDetail
                };

                await unitOfWork.TaskUpdateHistories.AddAsync(updateHistory);

                // Salva todas as mudanças
                await unitOfWork.CompleteAsync();
                await unitOfWork.CommitAsync(); // Confirma a transação

                return mapper.Map<TaskDto>(task);
            }
            catch
            {
                await unitOfWork.RollbackAsync(); // Reverte os updates em caso de qulquer erro
                throw; 
            }
        }


        [ExcludeFromCodeCoverage]
        private string GetUpdateDetails(Tasks task, UpdateTaskDto updateTaskDto)
        {
            var changes = new List<string>();

            if (task.Title != updateTaskDto.Title && updateTaskDto.Title != null)
                changes.Add($"Título alterado de '{task.Title}' para '{updateTaskDto.Title}'.");

            if (task.Description != updateTaskDto.Description && updateTaskDto.Description != null)
                changes.Add("Descrição alterada.");

            if (task.Status != updateTaskDto.Status && updateTaskDto.Status.HasValue)
                changes.Add($"Status alterado de {task.Status} para {updateTaskDto.Status}.");

            return string.Join(", ", changes);
        }
    }
}
