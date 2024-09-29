namespace CashFlow.Domain;

public interface IUsersRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository
{

}
