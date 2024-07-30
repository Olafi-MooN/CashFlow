using CashFlow.Communication;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Messages.Reports;
using FluentValidation;

namespace CashFlow.Application;

public class UserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public UserValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(ResourceReportGenerationMessages.NAME_REQUIRED);
        RuleFor(x => x.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        RuleFor(x => x.Email).NotEmpty().WithMessage(ResourceReportGenerationMessages.PASSWORD_REQUIRED).EmailAddress().WithMessage(ResourceReportGenerationMessages.EMAIL_REQUIRED);
    }
}
