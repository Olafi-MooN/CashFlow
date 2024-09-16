using System;
using AutoMapper;
using CashFlow.Application;
using CashFlow.Communication.Responses;
using CashFlow.Domain;
using CommonTestUtilities.Entities;
using FluentAssertions;
using UseCases.Tests.Repositories;

namespace UseCases.Tests.Expenses.GetAll;

public class GetAllExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var mapper = MapperBuilder.Build();
        var user = UserBuilder.Build();
        var expenses = ExpenseBuilder.Collection(user, 10);
        var response = mapper.Map<List<ResponseExpenseJson>>(expenses);

        var useCase = CreateUseCase(user, expenses, mapper);

        var result = await useCase.Execute();
        result.Should().NotBeNull();

        result.Expenses.Should().BeEquivalentTo(response);
    }

    public GetAllExpensesUseCase CreateUseCase(User user, List<Expense> expenses, IMapper mapper)
    {
        var repository = new ExpensesRepositoryBuilder().GetAllRead(user, expenses).Build();

        var loggedUser = new LoggedUserBuilder().Build(user).Build();

        return new GetAllExpensesUseCase(repository, mapper, loggedUser);
    }
}
