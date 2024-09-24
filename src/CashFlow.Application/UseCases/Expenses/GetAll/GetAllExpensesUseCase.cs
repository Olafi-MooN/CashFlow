using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application;

public class GetAllExpensesUseCase(IExpensesRepository repository, IMapper mapper, ILoggedUser loggedUser) : IGetAllExpensesUseCase
{
    private readonly IExpensesRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<ResponseExpensesJson> Execute(object? request = null)
    {
        var loggedUserResult = await _loggedUser.Get();
        var expenses = await _repository.GetAll(loggedUserResult);
        var response = new ResponseExpensesJson
        {
            Expenses = _mapper.Map<List<ResponseExpenseJson>>(expenses)
        };

        return response;
    }
}
