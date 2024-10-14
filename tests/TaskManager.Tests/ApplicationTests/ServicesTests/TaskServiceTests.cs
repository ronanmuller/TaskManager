using AutoMapper;
using Moq;
using TaskManager.Application.Dto;
using TaskManager.Application.Services;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces.Repositories;
using Xunit;

namespace TaskManager.Tests.ApplicationTests.ServicesTests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockUserService = new Mock<IUserService>();
            _mockMapper = new Mock<IMapper>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _taskService = new TaskService(
                _mockTaskRepository.Object,
                _mockProjectRepository.Object,
                _mockUserService.Object,
                _mockMapper.Object,
                _mockUnitOfWork.Object
            );
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldThrowArgumentException_WhenProjectNotFound()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto { ProjectId = 1 };

            _mockProjectRepository.Setup(pr => pr.ExistsAsync(createTaskDto.ProjectId))
                .ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _taskService.CreateTaskAsync(createTaskDto));
            Assert.Equal("Projeto com ID 1 não encontrado ou finalizado.", exception.Message);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldThrowInvalidOperationException_WhenTaskLimitExceeded()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto { ProjectId = 1 };
            _mockProjectRepository.Setup(pr => pr.ExistsAsync(createTaskDto.ProjectId))
                .ReturnsAsync(true);

            _mockTaskRepository.Setup(tr => tr.CountTasksByProjectIdAsync(createTaskDto.ProjectId))
                .ReturnsAsync(20); // Limite de 20 tarefas

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _taskService.CreateTaskAsync(createTaskDto));
            Assert.Equal("O projeto com ID 1 já possui o limite máximo de 20 tarefas.", exception.Message);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldReturnTaskDto_WhenTaskIsCreated()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto { ProjectId = 1, Title = "New Task" };
            var task = new Tasks { Id = 1, Title = "New Task", ProjectId = 1 };
            var taskDto = new TaskDto { Id = 1, Title = "New Task" };

            _mockProjectRepository.Setup(pr => pr.ExistsAsync(createTaskDto.ProjectId))
                .ReturnsAsync(true);

            _mockTaskRepository.Setup(tr => tr.CountTasksByProjectIdAsync(createTaskDto.ProjectId))
                .ReturnsAsync(0);

            _mockMapper.Setup(m => m.Map<Tasks>(createTaskDto)).Returns(task);
            _mockTaskRepository.Setup(tr => tr.AddAsync(task)).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<TaskDto>(task)).Returns(taskDto);

            // Act
            var result = await _taskService.CreateTaskAsync(createTaskDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskDto.Title, result.Title);
            _mockTaskRepository.Verify(tr => tr.AddAsync(task), Times.Once);
        }

        [Fact]
        public async Task GetTasksByProjectIdAsync_ShouldReturnTaskDtos()
        {
            // Arrange
            var projectId = 1;
            var tasks = new List<Tasks>
            {
                new Tasks { Id = 1, Title = "Task 1", ProjectId = projectId },
                new Tasks { Id = 2, Title = "Task 2", ProjectId = projectId }
            };
            var taskDtos = new List<TaskDto>
            {
                new TaskDto { Id = 1, Title = "Task 1" },
                new TaskDto { Id = 2, Title = "Task 2" }
            };

            _mockTaskRepository.Setup(tr => tr.GetTasksByProjectIdAsync(projectId, 0, 10))
                .ReturnsAsync(tasks);
            _mockMapper.Setup(m => m.Map<IEnumerable<TaskDto>>(tasks)).Returns(taskDtos);

            // Act
            var result = await _taskService.GetTasksByProjectIdAsync(projectId, 0, 10);

            // Assert
            Assert.Equal(2, result.Count());
            _mockTaskRepository.Verify(tr => tr.GetTasksByProjectIdAsync(projectId, 0, 10), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldThrowKeyNotFoundException_WhenTaskNotFound()
        {
            // Arrange
            var taskId = 1;
            var updateTaskDto = new UpdateTaskDto { Title = "Updated Title" };

            _mockUnitOfWork.Setup(uow => uow.Tasks.GetByIdAsync(taskId))
                .ReturnsAsync((Tasks)null); // Simulando que a tarefa não foi encontrada

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.UpdateTaskAsync(taskId, updateTaskDto));
            Assert.Equal("Tarefa com ID 1 não encontrada.", exception.Message);
        }
        [Fact]
        public async Task UpdateTaskAsync_ShouldReturnUpdatedTaskDto_WhenTaskIsUpdated()
        {
            // Arrange
            var taskId = 1;
            var updateTaskDto = new UpdateTaskDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
                Status = TaskState.InProgress
            };

            var existingTask = new Tasks
            {
                Id = taskId,
                Title = "Old Title",
                Description = "Old Description",
                Status = TaskState.Pending
            };

            var taskDto = new TaskDto
            {
                Id = taskId,
                Title = "Updated Title",
                Description = "Updated Description",
                Status = TaskState.InProgress
            };

            // Configurando o mock do repositório para retornar a tarefa existente
            _mockUnitOfWork.Setup(uow => uow.Tasks.GetByIdAsync(taskId)).ReturnsAsync(existingTask);

            // Configurando o mock do mapper para mapear o objeto atualizado para o DTO
            _mockMapper.Setup(m => m.Map<TaskDto>(existingTask)).Returns(taskDto);

            // Simulação da atualização da tarefa
            _mockUnitOfWork.Setup(uow => uow.Tasks.UpdateAsync(It.IsAny<Tasks>()))
                .Callback<Tasks>(t =>
                {
                    existingTask.Title = t.Title;
                    existingTask.Description = t.Description;
                    existingTask.Status = t.Status;
                });

            // Simulação do serviço de usuário
            _mockUserService.Setup(us => us.GetCurrentUserId()).Returns(1); // Simula que o ID do usuário atual é 1

            // Simulação da adição do histórico de atualizações
            _mockUnitOfWork.Setup(uow => uow.TaskUpdateHistories.AddAsync(It.IsAny<TaskUpdateHistory>()))
                .Returns(Task.CompletedTask); // Simula a operação de adição como concluída

            // Act
            var result = await _taskService.UpdateTaskAsync(taskId, updateTaskDto);

            // Assert
            Assert.NotNull(result); // Adicione esta verificação
            Assert.Equal("Updated Title", result.Title);
            Assert.Equal("Updated Description", result.Description);
            Assert.Equal(TaskState.InProgress, result.Status);
            _mockUnitOfWork.Verify(uow => uow.Tasks.UpdateAsync(It.IsAny<Tasks>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.TaskUpdateHistories.AddAsync(It.IsAny<TaskUpdateHistory>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

    }
}
