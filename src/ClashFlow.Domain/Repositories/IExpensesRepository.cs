namespace CashFlow.Domain;

public interface IExpensesRepository
{
    Task Add(Expense expense);
}
