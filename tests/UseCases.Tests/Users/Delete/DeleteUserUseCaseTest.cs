using CashFlow.Application.UseCases.Users.Delete;
using CashFlow.Domain;
using CommonTestUtilities.Entities;
using FluentAssertions;
using UseCases.Tests.Repositories;

namespace UseCases.Tests.Users.Delete;
public class DeleteUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute();

        await act.Should().NotThrowAsync();
    }

    private static DeleteUserUseCase CreateUseCase(User user)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = new LoggedUserBuilder().WithUser(user).Build();
        var usersRepository = new UsersRepositoryBuilder().Delete(user.Id);

        return new DeleteUserUseCase(usersRepository.Build(), loggedUser, unitOfWork);
    }
}
