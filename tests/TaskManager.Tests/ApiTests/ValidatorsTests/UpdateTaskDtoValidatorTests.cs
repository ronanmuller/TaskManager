using FluentValidation.TestHelper;
using TaskManager.Api.Validators;
using TaskManager.Application.Dto;
using TaskManager.Domain.Enums;
using Xunit;

namespace TaskManager.Tests.ApiTests.ValidatorsTests
{
    public class UpdateTaskDtoValidatorTests
    {
        private readonly UpdateTaskDtoValidator _validator;

        public UpdateTaskDtoValidatorTests()
        {
            _validator = new UpdateTaskDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Title_Exceeds_Max_Length()
        {
            var dto = new UpdateTaskDto { Title = new string('a', 201) };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("O título da tarefa deve ter no máximo 200 caracteres.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Title_Is_Valid()
        {
            var dto = new UpdateTaskDto { Title = "Valid Title" };
            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Exceeds_Max_Length()
        {
            var dto = new UpdateTaskDto { Description = new string('a', 1001) };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage("A descrição da tarefa deve ter no máximo 1000 caracteres.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Description_Is_Valid()
        {
            var dto = new UpdateTaskDto { Description = "Valid Description" };
            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Have_Error_When_DueDate_Is_In_The_Past()
        {
            var dto = new UpdateTaskDto { DueDate = DateTime.Now.AddDays(-1) };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.DueDate)
                .WithErrorMessage("A data de vencimento deve ser uma data futura.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_DueDate_Is_Valid()
        {
            var dto = new UpdateTaskDto { DueDate = DateTime.Now.AddDays(1) };
            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveValidationErrorFor(x => x.DueDate);
        }

        [Fact]
        public void Should_Have_Error_When_Status_Is_Invalid()
        {
            var dto = new UpdateTaskDto { Status = (TaskState)(-1) }; 
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Status)
                .WithErrorMessage("O status da tarefa é inválido.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Status_Is_Valid()
        {
            var dto = new UpdateTaskDto { Status = TaskState.Completed }; 
            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveValidationErrorFor(x => x.Status);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            var dto = new UpdateTaskDto
            {
                Title = "Valid Title",
                Description = "Valid Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = TaskState.Pending 
            };
            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
