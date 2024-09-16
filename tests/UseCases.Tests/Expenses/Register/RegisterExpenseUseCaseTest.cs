using System;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Requests;
using FluentAssertions;
using UseCases.Tests.Repositories;

namespace UseCases.Tests.Expenses.Register;

public class RegisterExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = new RequestRegisterExpensiveJsonBuilder().Build();
        var useCase = CreateUserCase();
        var result = await useCase.Execute(request);
        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var request = new RequestRegisterExpensiveJsonBuilder().Build();
        request.Title = string.Empty;
        var useCase = CreateUserCase();
        var result = async () => await useCase.Execute(request);

        await result.Should().ThrowAsync<ErrorOnValidateException>().Where(x => x.GetErrors().Count.Equals(1) && x.GetErrors().Contains(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Fact]
    public async Task Error_Amount_Zero()
    {
        var request = new RequestRegisterExpensiveJsonBuilder().Build();
        request.Amount = 0;
        var useCase = CreateUserCase();
        var result = async () => await useCase.Execute(request);

        await result.Should().ThrowAsync<ErrorOnValidateException>().Where(x => x.GetErrors().Count.Equals(1) && x.GetErrors().Contains(ResourceErrorMessages.AMOUNT_GREATER_THAN_ZERO));
    }

    private RegisterExpensiveUseCase CreateUserCase()
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var expensesRepository = new ExpensesRepositoryBuilder().Build();
        var loggedUser = new LoggedUserBuilder().Build();
        return new RegisterExpensiveUseCase(expensesRepository, unitOfWork, mapper, loggedUser);
    }
}
