using CashFlow.Application.Interfaces;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpensiveUseCase : IUseCase<RequestRegisterExpensiveJson, ResponseRegisteredExpenseJson>
{
    private readonly IExpensesRepository _repository;
    public RegisterExpensiveUseCase(IExpensesRepository repository)
    {
        _repository = repository;
    }
    public ResponseRegisteredExpenseJson Execute(RequestRegisterExpensiveJson request)
    {
        Validate(request);

        var entity = new Expense
        {
            Title = request.Title,
            Amount = request.Amount,
            Date = request.Date,
            Description = request.Description,
            PaymentType = (Domain.EPaymentTypeEnum)request.PaymentType
        };

        _repository.Add(entity);

        return new ResponseRegisteredExpenseJson { Title = request.Title };
    }

    private void Validate(RequestRegisterExpensiveJson request)
    {
        var messagesError = new RegisterExpensiveValidator().Validate(request).Errors.Select(x => x.ErrorMessage).ToList();
        if (messagesError.Count > 0)
            throw new ErrorOnValidateException(messagesError);
    }
}
