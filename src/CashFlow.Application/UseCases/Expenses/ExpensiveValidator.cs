using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class ExpensiveValidator : AbstractValidator<RequestRegisterExpensiveJson>
{
    public ExpensiveValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage(ResourceErrorMessages.TITLE_REQUIRED);
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_GREATER_THAN_ZERO);
        RuleFor(x => x.PaymentType).NotEmpty().WithMessage(ResourceErrorMessages.PAYMENT_TYPE_REQUIRED).IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_TYPE_ENUM);
        RuleFor(x => x.Tags)
            .ForEach(rule =>
            {
                var enumValues = Enum.GetValues(typeof(ETagTypeEnum)).Cast<ETagTypeEnum>();
                var enumNamesWithValues = string.Join(", ", enumValues.Select(v => $"{v}={(int) v}"));

                rule.IsInEnum().WithMessage(x => string.Format(ResourceErrorMessages.MUST_BE_IN_ENUM, nameof(ETagTypeEnum), enumNamesWithValues));
            });
    }
}
