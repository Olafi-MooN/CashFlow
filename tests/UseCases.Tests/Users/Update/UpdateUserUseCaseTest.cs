using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Domain;
using CashFlow.Domain.Messages.Reports;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using FluentAssertions;
using UseCases.Tests.Repositories;

namespace UseCases.Tests.Users.Update;
public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonFakerBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();

        user.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonFakerBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidateException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceReportGenerationMessages.NAME_REQUIRED));
    }

    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonFakerBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidateException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
    }

    private static UpdateUserUseCase CreateUseCase(User user, string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = new LoggedUserBuilder().WithUser(user).Build();
        var usersRepository = new UsersRepositoryBuilder().GetById(user);

        if (!string.IsNullOrEmpty(email)) usersRepository.ExistActiveUserWithEmail(email);

        return new UpdateUserUseCase(usersRepository.Build(), loggedUser, unitOfWork);
    }
}
