using FluentValidation.TestHelper;
using TaskManager.Api.Validators;
using TaskManager.Application.Dto;
using Xunit;

namespace TaskManager.Tests.ApiTests.ValidatorsTests
{
    public class CreateProjectDtoValidatorTests
    {
        private readonly CreateProjectDtoValidator _validator;

        public CreateProjectDtoValidatorTests()
        {
            _validator = new CreateProjectDtoValidator();
        }

        [Fact]
        public void ShouldHaveValidationErrorFor_Name_WhenEmpty()
        {
            // Arrange
            var dto = new CreateProjectDto { Name = "", UserId = 1 };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("O nome do projeto é obrigatório.");
        }

        [Fact]
        public void ShouldHaveValidationErrorFor_Name_WhenTooLong()
        {
            // Arrange
            var dto = new CreateProjectDto { Name = new string('a', 101), UserId = 1 };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("O nome do projeto não pode ter mais de 100 caracteres.");
        }

        [Fact]
        public void ShouldHaveValidationErrorFor_UserId_WhenLessThanOrEqualToZero()
        {
            // Arrange
            var dto = new CreateProjectDto { Name = "Valid Name", UserId = 0 };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId)
                .WithErrorMessage("O ID do usuário deve ser maior que zero.");
        }
    }
}