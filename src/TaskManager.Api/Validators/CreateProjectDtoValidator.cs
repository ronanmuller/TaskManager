using FluentValidation;
using TaskManager.Application.Dto;

namespace TaskManager.Api.Validators;

public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("O nome do projeto é obrigatório.")
            .MaximumLength(100).WithMessage("O nome do projeto não pode ter mais de 100 caracteres.");

        RuleFor(dto => dto.UserId)
            .GreaterThan(0).WithMessage("O ID do usuário deve ser maior que zero.");
    }
}