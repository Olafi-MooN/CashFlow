using AutoMapper;
using CashFlow.Application;
using CashFlow.Domain;
using CashFlow.Exception;
using CommonTestUtilities.Entities;
using FluentAssertions;
using UseCases.Tests.Repositories;

namespace UseCases.Tests.Expenses.DeleteById;

public class DeleteByIdExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var mapper = MapperBuilder.Build();
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);

        var useCase = CreateUseCase(mapper, expense, user);
        var result = await useCase.Execute(expense.Id);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var mapper = MapperBuilder.Build();
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);

        var useCase = CreateUseCase(mapper, expense, user);
        var result = async () => await useCase.Execute(int.MinValue);

        await result.Should().ThrowAsync<NotFoundException>().Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }


    private DeleteExpenseById CreateUseCase(IMapper mapper, Expense expense, User user)
    {
        var repository = new ExpensesRepositoryBuilder().GetById(user.Id, expense).DeleteById(expense).Build();
        var loggedUser = new LoggedUserBuilder().Build(user).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new DeleteExpenseById(repository, unitOfWork, loggedUser);
    }
}
