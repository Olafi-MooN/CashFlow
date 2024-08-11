using System;
using CashFlow.Application;
using CashFlow.Communication;
using CashFlow.Domain;
using CashFlow.Exception;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Tests.Login;

public class DoLoginUseCaseTest
{

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var mapper = MapperBuilder.Build();
        var request = mapper.Map<RequestLoginJson>(user);
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Email_Not_Found()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        var useCase = CreateUseCase(user);

        var result = async () => await useCase.Execute(request);

        await result.Should().ThrowAsync<InvalidLoginException>();
    }

    [Fact]
    public async Task Error_Password_Not_Match()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidLoginException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));
    }

    private DoLoginUseCase CreateUseCase(User user = default!)
    {
        var repository = new UsersRepositoryBuilder().GetByEmail(user).Build();
        var encryptPassword = new EncryptPasswordBuilder().Verify(user.Password).Build();
        var accessTokenGenerator = AccessTokenGeneratorBuilder.Build();

        return new DoLoginUseCase(repository, encryptPassword, accessTokenGenerator);
    }
}
