using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain;
using CashFlow.Exception;

namespace CashFlow.Application;

public class GetByIdExpenseUseCase : IGetByIdExpenseUseCase
{
    private readonly IExpensesRepository _repository;
    private readonly IMapper _mapper;
    public GetByIdExpenseUseCase(IExpensesRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ResponseExpenseJson> Execute(long request)
    {
        var expense = await _repository.GetById(request);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        return _mapper.Map<ResponseExpenseJson>(expense);
    }

    public Task<ResponseExpenseJson> Execute()
    {
        throw new NotImplementedException();
    }
}
