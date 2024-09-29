using AutoMapper;
using CashFlow.Application;
using CashFlow.Communication.Responses;
using CashFlow.Domain;
using CashFlow.Exception;
using CommonTestUtilities.Entities;
using FluentAssertions;
using UseCases.Tests.Repositories;

namespace UseCases.Tests.Expenses.GetById;

public class GetByIdExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var mapper = MapperBuilder.Build();
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);
        var response = mapper.Map<ResponseExpenseJson>(expense);

        var useCase = CreateUseCase(user: user, mapper: mapper, expense: expense);

        var result = await useCase.Execute(expense.Id);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var mapper = MapperBuilder.Build();
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);
        var useCase = CreateUseCase(user: user, mapper: mapper);

        var result = async () => await useCase.Execute(expense.Id);
        await result.Should().ThrowAsync<NotFoundException>().Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }

    private GetByIdExpenseUseCase CreateUseCase(User user, IMapper mapper, Expense? expense = null)
    {
        var repository = new ExpensesRepositoryBuilder().GetById(user.Id, expense).Build();
        var loggedUser = new LoggedUserBuilder().WithUser(user).Build();

        return new GetByIdExpenseUseCase(repository, mapper, loggedUser);
    }
}
