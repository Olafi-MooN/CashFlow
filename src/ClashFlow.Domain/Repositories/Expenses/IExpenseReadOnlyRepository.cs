namespace CashFlow.Domain;

public interface IExpenseReadOnlyRepository
{
    Task<List<Expense>> GetAll();
    Task<Expense?> GetById(long id);
    Task<List<Expense>> FilterByMonth(DateOnly date);

}
