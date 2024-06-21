using CashFlow.Application.Interfaces;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpensiveUseCase : IUseCase<RequestRegisterExpensiveJson, Task<ResponseRegisteredExpenseJson>>
{
    private readonly IExpensesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public RegisterExpensiveUseCase(IExpensesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseRegisteredExpenseJson> Execute(RequestRegisterExpensiveJson request)
    {
        Validate(request);

        var entity = new Expense
        {
            Title = request.Title,
            Amount = request.Amount,
            Date = request.Date,
            Description = request.Description,
            PaymentType = (EPaymentTypeEnum)request.PaymentType
        };

        await _repository.Add(entity);
        await _unitOfWork.Commit();

        return new ResponseRegisteredExpenseJson { Title = request.Title };
    }

    private void Validate(RequestRegisterExpensiveJson request)
    {
        var messagesError = new RegisterExpensiveValidator().Validate(request).Errors.Select(x => x.ErrorMessage).ToList();
        if (messagesError.Count > 0)
            throw new ErrorOnValidateException(messagesError);
    }
}
