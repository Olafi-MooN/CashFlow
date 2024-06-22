using CashFlow.Domain;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure;

internal class ExpensesRepository : IExpensesRepository
{
    private readonly CashFlowDbContext _dbContext;

    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);
    }

    public async Task<List<Expense>> GetAll()
    {
        return await _dbContext.Expenses.AsNoTracking().ToListAsync();
    }

    public async Task<Expense?> GetById(long id)
    {
        var response = await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return response;
    }
}
