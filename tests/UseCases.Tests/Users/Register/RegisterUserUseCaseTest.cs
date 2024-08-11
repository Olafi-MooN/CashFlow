using CashFlow.Application;
using CashFlow.Domain.Messages.Reports;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities;
using FluentAssertions;
using FluentValidation;

namespace UseCases.Tests.Users.Register;

public class RegisterUserUseCaseTest
{

    [Fact]
    public async Task Success()
    {
        var request = RequestsRegisterUserJsonBuilder.Build();
        var useCase = CreateUserCase();
        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestsRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUserCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidateException>();

        result.Where(x => x.GetErrors().Count.Equals(1) && x.GetErrors().Contains(ResourceReportGenerationMessages.NAME_REQUIRED));
    }

    [Fact]
    public async Task Error_Email_Empty()
    {
        var request = RequestsRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var useCase = CreateUserCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidateException>();

        result.Where(x => x.GetErrors().Count.Equals(1) && x.GetErrors().Contains(ResourceReportGenerationMessages.EMAIL_REQUIRED));
    }
    [Fact]
    public async Task Error_Email_Already_Exist()
    {
        var request = RequestsRegisterUserJsonBuilder.Build();
        var useCase = CreateUserCase(request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidateException>();
        result.Where(x => x.GetErrors().Count.Equals(1) && x.GetErrors().Contains(ResourceReportGenerationMessages.USER_ALREADY_EXISTS));
    }


    private RegisterUserUseCase CreateUserCase(string email = "")
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var usersRepository = new UsersRepositoryBuilder();
        var encryptPassword = new EncryptPasswordBuilder().Build();
        var accessTokenGenerator = AccessTokenGeneratorBuilder.Build();

        if (!string.IsNullOrWhiteSpace(email)) usersRepository.ExistActiveUserWithEmail(email);

        return new RegisterUserUseCase(unitOfWork, usersRepository.Build(), mapper, encryptPassword, accessTokenGenerator);
    }


}
