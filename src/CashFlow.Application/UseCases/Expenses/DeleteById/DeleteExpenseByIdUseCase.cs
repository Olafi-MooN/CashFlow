using CashFlow.Domain;
using CashFlow.Exception;

namespace CashFlow.Application;

public class DeleteExpenseById : IDeleteExpenseByIdUseCase
{
    private readonly IExpensesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExpenseById(IExpensesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Execute(long id = 0)
    {
        var result = await _repository.DeleteById(id);
        if (result is false) throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        await _unitOfWork.Commit();

        return result;
    }
}
