using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Domain;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using FluentAssertions;
using UseCases.Tests.Repositories;

namespace UseCases.Tests.Users.ChangePassword;
public class ChangePasswordUseCaseTest
{

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonFakerBuilder.Build();

        var useCase = CreateUseCase(user, request.OldPassword);

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_OldPassword_IsInvalid()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonFakerBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidateException>();

        result.Where(x => x.GetErrors().Count.Equals(1) && x.GetErrors().Contains(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

    }

    private static ChangePasswordUseCase CreateUseCase(User user, string? password = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = new LoggedUserBuilder().WithUser(user).Build();
        var usersRepository = new UsersRepositoryBuilder().GetById(user);
        var encryptPassword = new EncryptPasswordBuilder().Verify(password).Build();

        return new ChangePasswordUseCase(loggedUser, usersRepository.Build(), unitOfWork, encryptPassword);
    }
}
