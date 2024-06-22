using AutoMapper;
using CashFlow.Application.DTOs;
using CashFlow.Application.Interfaces;
using CashFlow.Communication.Responses;
using CashFlow.Domain;

namespace CashFlow.Application;

public class GetAllExpensesUseCase : IGetAllExpensesUseCase
{
    private readonly IExpensesRepository _repository;
    private readonly IMapper _mapper;

    public GetAllExpensesUseCase(IExpensesRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseExpensesJson> Execute()
    {
        var expenses = await _repository.GetAll();
        var response = new ResponseExpensesJson
        {
            Expenses = _mapper.Map<List<ResponseExpenseJson>>(expenses)
        };

        return response;
    }

    public Task<ResponseExpensesJson> Execute(object request)
    {
        throw new NotImplementedException();
    }
}
