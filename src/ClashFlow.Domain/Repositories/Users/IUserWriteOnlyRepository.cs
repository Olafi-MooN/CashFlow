namespace CashFlow.Domain;

public interface IUserWriteOnlyRepository
{
    public Task<User> Add(User user);
}
