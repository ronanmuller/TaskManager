using FluentValidation;
using TaskManager.Application.Dto;

namespace TaskManager.Api.Validators;

public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskDtoValidator()
    {
        // Valida o ProjectId
        RuleFor(x => x.ProjectId)
            .GreaterThan(0).WithMessage("O ID do projeto deve ser maior que zero.");

        // Valida o Title
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("O título da tarefa é obrigatório.")
            .MaximumLength(100).WithMessage("O título da tarefa deve ter no máximo 100 caracteres.");

        // Valida a Description
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição da tarefa é obrigatória.");

        // Valida o DueDate (data de vencimento não pode ser no passado)
        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage("A data de vencimento deve ser uma data futura.");

        // Valida o Status
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("O status da tarefa é inválido.");

        // Valida a Priority
        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("A prioridade da tarefa é inválida.");
    }
}