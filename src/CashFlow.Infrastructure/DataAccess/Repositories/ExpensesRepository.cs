using CashFlow.Domain;

namespace CashFlow.Infrastructure;

internal class ExpensesRepository : IExpensesRepository
{
    public void Add(Expense expense)
    {
        var DbContext = new CashFlowDbContext();
        DbContext.Expenses.Add(expense);
        DbContext.SaveChanges();
    }
}
