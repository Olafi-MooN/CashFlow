using CashFlow.Domain;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;

namespace CashFlow.Application;

public class DeleteExpenseById : IDeleteExpenseByIdUseCase
{
    private readonly IExpensesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public DeleteExpenseById(IExpensesRepository repository, IUnitOfWork unitOfWork, ILoggedUser loggedUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }
    public async Task<bool> Execute(long request = default!)
    {
        var loggedUserResult = await _loggedUser.Get();
        var expenses = await (_repository as IExpenseReadOnlyRepository).GetByIdRead(request, loggedUserResult.Id);

        if (expenses is null || expenses.UserId != loggedUserResult.Id) throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        var result = await _repository.DeleteById(request);
        await _unitOfWork.Commit();

        return result;
    }
}
