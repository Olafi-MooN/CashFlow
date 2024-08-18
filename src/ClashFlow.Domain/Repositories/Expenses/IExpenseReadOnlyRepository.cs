namespace CashFlow.Domain;

public interface IExpenseReadOnlyRepository
{
    Task<List<Expense>> GetAll(User user);
    Task<Expense?> GetByIdRead(long id, Guid userId);
    Task<List<Expense>> FilterByMonth(DateOnly date, Guid userId);

}
