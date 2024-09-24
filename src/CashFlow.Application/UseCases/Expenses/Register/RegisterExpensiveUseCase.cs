using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpensiveUseCase(IExpensesRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILoggedUser loggedUser) : IRegisterExpenseUseCase
{
    private readonly IExpensesRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<ResponseRegisteredExpenseJson> Execute(RequestRegisterExpensiveJson request = default!)
    {
        Validate(request);

        var loggedUserResult = await _loggedUser.Get();

        var entity = _mapper.Map<Expense>(request);

        entity.UserId = loggedUserResult.Id;

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
