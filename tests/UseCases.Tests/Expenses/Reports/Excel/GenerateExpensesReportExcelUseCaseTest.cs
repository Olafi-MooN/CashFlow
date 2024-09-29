using CashFlow.Application;
using CashFlow.Communication;
using CashFlow.Domain;
using CommonTestUtilities.Entities;
using FluentAssertions;
using UseCases.Tests.Repositories;

namespace UseCases.Tests.Expenses.Reports.Excel;
public class GenerateExpensesReportExcelUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expenses = ExpenseBuilder.Collection(loggedUser);

        var useCase = CreateUseCase(loggedUser, expenses);

        var result = await useCase.Execute(new RequestInFormationReportJson() { Month = DateOnly.FromDateTime(DateTime.Today) });

        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Success_Empty()
    {
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser, []);

        var result = await useCase.Execute(new RequestInFormationReportJson() { Month = DateOnly.FromDateTime(DateTime.Today) });

        result.Should().BeEmpty();
    }

    private static GenerateExpenseReportExcelUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var repository = new ExpensesRepositoryBuilder().GetByIdMonth(user.Id, expenses).Build();
        var loggedUser = new LoggedUserBuilder().WithUser(user).Build();

        return new GenerateExpenseReportExcelUseCase(repository, loggedUser);
    }
}

