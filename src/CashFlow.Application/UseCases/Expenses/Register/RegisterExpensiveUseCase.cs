using AutoMapper;
using CashFlow.Application.Interfaces;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpensiveUseCase(IExpensesRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : IRegisterExpenseUseCase
{
    private readonly IExpensesRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseRegisteredExpenseJson> Execute(RequestRegisterExpensiveJson request)
    {
        Validate(request);

        var entity = _mapper.Map<Expense>(request);

        await _repository.Add(entity);
        await _unitOfWork.Commit();

        return new ResponseRegisteredExpenseJson { Title = request.Title };
    }

    private void Validate(RequestRegisterExpensiveJson request)
    {
        var messagesError = new ExpensiveValidator().Validate(request).Errors.Select(x => x.ErrorMessage).ToList();
        if (messagesError.Count > 0)
            throw new ErrorOnValidateException(messagesError);
    }
}
