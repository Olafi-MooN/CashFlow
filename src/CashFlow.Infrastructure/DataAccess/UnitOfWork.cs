using CashFlow.Domain;

namespace CashFlow.Infrastructure;

internal class UnitOfWork(CashFlowDbContext dbContext) : IUnitOfWork
{
    private readonly CashFlowDbContext _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
