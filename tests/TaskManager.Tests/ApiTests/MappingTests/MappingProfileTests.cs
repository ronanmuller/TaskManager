using AutoMapper;
using TaskManager.Application.Dto;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using Xunit;

namespace TaskManager.Api.Mapping
{
    public class MappingProfileTests
    {
        private readonly IMapper _mapper;

        public MappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void Project_To_ProjectDto_Mapping_ShouldBeValid()
        {
            // Arrange
            var project = new Project { Id = 1, Name = "Project 1", UserId = 1 };

            // Act
            var projectDto = _mapper.Map<ProjectDto>(project);

            // Assert
            Assert.Equal(project.Id, projectDto.Id);
            Assert.Equal(project.Name, projectDto.Name);
            Assert.Equal(project.UserId, projectDto.UserId);
        }

        [Fact]
        public void Task_To_TaskDto_Mapping_ShouldBeValid()
        {
            // Arrange
            var task = new Tasks { Id = 1, Title = "Task 1", Status = TaskState.Completed };

            // Act
            var taskDto = _mapper.Map<TaskDto>(task);

            // Assert
            Assert.Equal(task.Id, taskDto.Id);
            Assert.Equal(task.Title, taskDto.Title);
            Assert.Equal(task.Status, taskDto.Status);
        }

        [Fact]
        public void CreateTaskDto_To_Tasks_Mapping_ShouldSetStatusAsPending()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto { Title = "New Task", Status = TaskState.Pending };

            // Act
            var task = _mapper.Map<Tasks>(createTaskDto);

            // Assert
            Assert.Equal(createTaskDto.Title, task.Title);
            Assert.Equal(TaskState.Pending, task.Status); // Verifica se o status é definido como Pending
        }

        [Fact]
        public void Task_To_TaskReportDto_Mapping_ShouldBeValid()
        {
            // Arrange
            var task = new Tasks
            {
                Id = 1,
                Title = "Task 1",
                Project = new Project { UserId = 1 }
            };

            // Act
            var taskReportDto = _mapper.Map<TaskReportDto>(task);

            // Assert
            Assert.Equal(task.Project.UserId, taskReportDto.UserId);
        }

        [Fact]
        public void TaskComment_To_TaskCommentDto_Mapping_ShouldBeValid()
        {
            // Arrange
            var comment = new TaskComment { Id = 1, Comment = "Comment 1", CreatedAt = DateTime.Now, UserId = 1 };

            // Act
            var commentDto = _mapper.Map<TaskCommentDto>(comment);

            // Assert
            Assert.Equal(comment.Id, commentDto.Id);
            Assert.Equal(comment.Comment, commentDto.Content);
            Assert.Equal(comment.CreatedAt, commentDto.CreatedDate);
            Assert.Equal(comment.UserId, commentDto.UserId);
        }
    }
}
