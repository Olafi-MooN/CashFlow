using CashFlow.Domain;

namespace CashFlow.Infrastructure;

internal class UsersRepository : IUsersRepository
{
    private readonly CashFlowDbContext _dbContext;
    public UsersRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> Add(User user)
    {
        var response = await _dbContext.Users.AddAsync(user);
        return response.Entity;
    }
}
