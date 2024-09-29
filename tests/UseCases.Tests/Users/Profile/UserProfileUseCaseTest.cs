using CashFlow.Application.UseCases.Users.Profile;
using CashFlow.Communication;
using CashFlow.Domain;
using CommonTestUtilities.Entities;
using FluentAssertions;
using UseCases.Tests.Repositories;

namespace UseCases.Tests.Users.Profile;
public class UserProfileUseCaseTest
{

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute();

        var expected = new ResponseUserProfileJson { Email = user.Email, Name = user.Name };

        result.Should().BeEquivalentTo(expected);
    }

    private static GetUserProfileUseCase CreateUseCase(User user)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = new LoggedUserBuilder().WithUser(user).Build();

        return new GetUserProfileUseCase(loggedUser, mapper);
    }
}
