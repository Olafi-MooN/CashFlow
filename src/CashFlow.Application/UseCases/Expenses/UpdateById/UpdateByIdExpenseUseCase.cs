using AutoMapper;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication;
using CashFlow.Communication.Responses;
using CashFlow.Domain;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application;

public class UpdateByIdExpenseUseCase : IUpdateByIdExpenseUseCase
{
    private readonly IExpensesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private readonly ILoggedUser _loggedUser;

    public UpdateByIdExpenseUseCase(
        IExpensesRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggedUser LoggedUser
    )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggedUser = LoggedUser;
    }

    public async Task<ResponseExpenseJson> Execute(RequestUpdateExpenseJson request = default!, params object[] parameters)
    {
        Validate(request);
        var loggedUserResult = await _loggedUser.Get();
        var expense = await (_repository as IExpensesUpdateOnlyRepository).GetByIdUpdate(long.Parse(parameters[0].ToString()!), loggedUserResult.Id) ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        expense.Tags.Clear();

        await _repository.UpdateById(_mapper.Map(request, expense));
        await _unitOfWork.Commit();

        return _mapper.Map<ResponseExpenseJson>(expense);
    }

    public Task<ResponseExpenseJson> Execute(RequestUpdateExpenseJson request = default!)
    {
        throw new NotImplementedException();
    }

    private void Validate(RequestUpdateExpenseJson request)
    {
        var messagesError = new ExpensiveValidator().Validate(request).Errors.Select(x => x.ErrorMessage).ToList();
        if (messagesError.Count > 0)
            throw new ErrorOnValidateException(messagesError);
    }
}
