using FluentValidation.TestHelper;
using TaskManager.Api.Validators;
using TaskManager.Application.Dto;
using TaskManager.Domain.Enums;
using Xunit;

namespace TaskManager.Tests.ApiTests.ValidatorsTests
{
    public class CreateTaskDtoValidatorTests
    {
        private readonly CreateTaskDtoValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_ProjectId_Is_Zero()
        {
            var dto = new CreateTaskDto { ProjectId = 0 };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.ProjectId)
                .WithErrorMessage("O ID do projeto deve ser maior que zero.");
        }

        [Fact]
        public void Should_Have_Error_When_Title_Is_Empty()
        {
            var dto = new CreateTaskDto { Title = "", ProjectId = 1 };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("O título da tarefa é obrigatório.");
        }

        [Fact]
        public void Should_Have_Error_When_Title_Exceeds_Max_Length()
        {
            var dto = new CreateTaskDto { Title = new string('a', 101), ProjectId = 1 };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("O título da tarefa deve ter no máximo 100 caracteres.");
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Empty()
        {
            var dto = new CreateTaskDto { Description = "", ProjectId = 1, Title = "Task Title" };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage("A descrição da tarefa é obrigatória.");
        }

        [Fact]
        public void Should_Have_Error_When_DueDate_Is_In_The_Past()
        {
            var dto = new CreateTaskDto { DueDate = DateTime.Now.AddDays(-1), ProjectId = 1, Title = "Task Title", Description = "Task Description" };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.DueDate)
                .WithErrorMessage("A data de vencimento deve ser uma data futura.");
        }

        [Fact]
        public void Should_Have_Error_When_Status_Is_Invalid()
        {
            var dto = new CreateTaskDto { Status = (TaskState)(-1), ProjectId = 1, Title = "Task Title", Description = "Task Description", DueDate = DateTime.Now.AddDays(1) };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Status)
                .WithErrorMessage("O status da tarefa é inválido.");
        }

        [Fact]
        public void Should_Have_Error_When_Priority_Is_Invalid()
        {
            var dto = new CreateTaskDto { Priority = (TaskPriority)(-1), ProjectId = 1, Title = "Task Title", Description = "Task Description", DueDate = DateTime.Now.AddDays(1) };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Priority)
                .WithErrorMessage("A prioridade da tarefa é inválida.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Valid_Dto()
        {
            var dto = new CreateTaskDto
            {
                ProjectId = 1,
                Title = "Task Title",
                Description = "Task Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = TaskState.Pending,
                Priority = TaskPriority.Medium
            };
            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
