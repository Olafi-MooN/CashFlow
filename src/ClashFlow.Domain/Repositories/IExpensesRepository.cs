namespace CashFlow.Domain;

public interface IExpensesRepository
{
    Task Add(Expense expense);
    Task<List<Expense>> GetAll();
    Task<Expense?> GetById(long id);
    Task<bool> DeleteById(long id);
    Task<bool> UpdateById(Expense expense);
}
