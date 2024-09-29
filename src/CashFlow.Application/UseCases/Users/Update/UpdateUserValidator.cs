using CashFlow.Communication;
using CashFlow.Domain.Messages.Reports;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users.Update;
public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(ResourceReportGenerationMessages.NAME_REQUIRED);
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ResourceReportGenerationMessages.EMAIL_REQUIRED)
            .EmailAddress().When(user => !string.IsNullOrWhiteSpace(user.Email), ApplyConditionTo.CurrentValidator).WithMessage(ResourceReportGenerationMessages.EMAIL_REQUIRED);
    }
}
