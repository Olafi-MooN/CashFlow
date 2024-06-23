using AutoMapper;
using CashFlow.Application.DTOs;
using CashFlow.Application.Interfaces;
using CashFlow.Communication.Responses;
using CashFlow.Domain;

namespace CashFlow.Application;

public class GetAllExpensesUseCase(IExpensesRepository repository, IMapper mapper) : IGetAllExpensesUseCase
{
    private readonly IExpensesRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseExpensesJson> Execute(object? request = null)
    {
        var expenses = await _repository.GetAll();
        var response = new ResponseExpensesJson
        {
            Expenses = _mapper.Map<List<ResponseExpenseJson>>(expenses)
        };

        return response;
    }
}
