using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain;
using CashFlow.Exception;

namespace CashFlow.Application;

public class GetByIdExpenseUseCase(IExpensesRepository repository, IMapper mapper) : IGetByIdExpenseUseCase
{
    private readonly IExpensesRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseExpenseJson> Execute(long request)
    {
        var expense = await _repository.GetById(request);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        return _mapper.Map<ResponseExpenseJson>(expense);
    }
}
