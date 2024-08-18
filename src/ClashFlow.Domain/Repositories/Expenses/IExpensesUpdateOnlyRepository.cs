namespace CashFlow.Domain;

public interface IExpensesUpdateOnlyRepository
{
    Task<bool> UpdateById(Expense expense);
    Task<Expense?> GetByIdUpdate(long id, Guid userId);
}
