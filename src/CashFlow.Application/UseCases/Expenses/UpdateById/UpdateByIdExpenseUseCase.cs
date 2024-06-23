using AutoMapper;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication;
using CashFlow.Domain;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application;

public class UpdateByIdExpenseUseCase : IUpdateByIdExpenseUseCase
{
    private readonly IExpensesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateByIdExpenseUseCase(
        IExpensesRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<object> Execute(RequestUpdateExpenseJson request)
    {
        await Task.Delay(0);
        Validate(request);


        return new object();
    }

    private void Validate(RequestUpdateExpenseJson request)
    {
        var messagesError = new ExpensiveValidator().Validate(request).Errors.Select(x => x.ErrorMessage).ToList();
        if (messagesError.Count > 0)
            throw new ErrorOnValidateException(messagesError);
    }
}
