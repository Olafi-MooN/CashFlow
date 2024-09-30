using CashFlow.Domain;
using Moq;

namespace UseCases.Tests;

public class UsersRepositoryBuilder
{
    private readonly Mock<IUsersRepository> _repository;

    public UsersRepositoryBuilder()
    {
        _repository = new Mock<IUsersRepository>();
    }

    public UsersRepositoryBuilder ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(x => x.ExistActiveUserWithEmail(email)).ReturnsAsync(true);

        return this;
    }

    public UsersRepositoryBuilder GetByEmail(User user)
    {
        _repository.Setup(x => x.GetByEmail(user.Email)).ReturnsAsync(user);
        return this;
    }


    public UsersRepositoryBuilder GetById(User user)
    {
        _repository.Setup(x => x.GetById(user.Id)).ReturnsAsync(user);
        return this;
    }

    public UsersRepositoryBuilder Delete(Guid id)
    {
        _repository.Setup(x => x.Delete(id));
        return this;
    }

    public IUsersRepository Build() => _repository.Object;
}
