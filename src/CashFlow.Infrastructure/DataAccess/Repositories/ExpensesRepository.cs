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

    async Task<Expense?> IExpenseReadOnlyRepository.GetById(long id)
    {
        var response = await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return response;
    }

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(long id)
    {
        var response = await _dbContext.Expenses.FirstOrDefaultAsync(x => x.Id == id);
        return response;
    }

    public async Task<bool> DeleteById(long id)
    {
        var response = await _dbContext.Expenses.FirstOrDefaultAsync(x => x.Id == id);

        if (response is null) return false;

        _dbContext.Expenses.Remove(response);

        return true;
    }

    public async Task<bool> UpdateById(Expense expense)
    {

        var response = await _dbContext.Expenses.FirstOrDefaultAsync(x => x.Id == expense.Id);

        if (response is null) return false;

        var result = _dbContext.Expenses.Update(expense);
        return true;
    }

    public async Task<List<Expense>> FilterByMonth(DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        return await _dbContext
          .Expenses
          .AsNoTracking()
          .Where(expense => expense.Date >= startDate && expense.Date <= endDate)
          .OrderBy(expense => expense.Date)
          .ThenBy(expense => expense.Title)
          .ToListAsync();
    }
}
