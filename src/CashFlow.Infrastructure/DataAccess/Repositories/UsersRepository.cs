using CashFlow.Domain;
using Microsoft.EntityFrameworkCore;

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

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext.Users.AnyAsync(x => x.Email == email);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(email));
    }

    public async Task<User?> GetById(Guid id)
    {
        return await _dbContext.Users.FirstAsync(x => x.Id.Equals(id));
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }

    public async Task Delete(Guid id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        _dbContext.Users.Remove(user!);
    }
}
