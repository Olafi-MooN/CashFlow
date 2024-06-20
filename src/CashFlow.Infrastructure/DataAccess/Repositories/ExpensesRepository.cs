using CashFlow.Domain;

namespace CashFlow.Infrastructure;

internal class ExpensesRepository : IExpensesRepository
{
    private readonly CashFlowDbContext _dbContext;
    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Add(Expense expense)
    {
        _dbContext.Expenses.Add(expense);
        _dbContext.SaveChanges();
    }
}
