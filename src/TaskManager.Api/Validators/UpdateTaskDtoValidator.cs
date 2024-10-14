using FluentValidation;
using TaskManager.Application.Dto;

namespace TaskManager.Api.Validators;

public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskDtoValidator()
    {
        // Valida o tamanho do Title
        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("O título da tarefa deve ter no máximo 200 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Title)); // Validação somente se o Title estiver presente

        // Valida o tamanho da Description
        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("A descrição da tarefa deve ter no máximo 1000 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Description)); // Validação somente se a Description estiver presente

        // Valida o DueDate
        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage("A data de vencimento deve ser uma data futura.")
            .When(x => x.DueDate.HasValue); // Validação somente se a DueDate estiver presente

        // Valida o Status (caso seja passado)
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("O status da tarefa é inválido.")
            .When(x => x.Status.HasValue); // Validação somente se o Status estiver presente
    }
}