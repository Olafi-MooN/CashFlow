using CashFlow.Application;
using CashFlow.Communication;
using CashFlow.Domain;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using FluentAssertions;
using UseCases.Tests.Repositories;

namespace UseCases.Tests.Expenses.UpdateById;

public class UpdateByIdExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, expense);

        var act = async () => await useCase.Execute(request, expense.Id);

        await act.Should().NotThrowAsync();

        expense.Title.Should().Be(request.Title);
        expense.Description.Should().Be(request.Description);
        expense.Date.Should().Be(request.Date);
        expense.Amount.Should().Be(request.Amount);
        expense.PaymentType.Should().Be((EPaymentTypeEnum) request.PaymentType);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(loggedUser, expense);

        var act = async () => await useCase.Execute(request, expense.Id);

        var result = await act.Should().ThrowAsync<ErrorOnValidateException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();

        var expense = ExpenseBuilder.Build(loggedUser);

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(loggedUser, expense);

        var act = async () => await useCase.Execute(request, long.MinValue);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }

    private UpdateByIdExpenseUseCase CreateUseCase(User user, Expense expense = default!, RequestUpdateExpenseJson? requestUpdateExpenseJson = null)
    {
        var mapper = MapperBuilder.Build();

        var repository = new ExpensesRepositoryBuilder().GetByIdUpdate(expense.Id, user.Id, expense).UpdateById(mapper.Map<Expense>(requestUpdateExpenseJson)).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = new LoggedUserBuilder().WithUser(user).Build();

        return new UpdateByIdExpenseUseCase(repository, unitOfWork, mapper, loggedUser);
    }
}
