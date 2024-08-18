using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain;
using CashFlow.Domain.Security.token;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;

namespace CashFlow.Application;

public class GetByIdExpenseUseCase(IExpensesRepository repository, IMapper mapper, ILoggedUser loggedUser) : IGetByIdExpenseUseCase
{
    private readonly IExpensesRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<ResponseExpenseJson> Execute(long request = default!)
    {
        var loggedUserResult = await _loggedUser.Get();

        var expense = await (_repository as IExpenseReadOnlyRepository).GetByIdRead(request, loggedUserResult.Id) ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        return _mapper.Map<ResponseExpenseJson>(expense);
    }
}
