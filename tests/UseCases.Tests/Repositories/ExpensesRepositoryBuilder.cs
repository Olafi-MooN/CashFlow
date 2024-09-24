using CashFlow.Domain;
using Moq;

namespace UseCases.Tests.Repositories;

public class ExpensesRepositoryBuilder
{
    private readonly Mock<IExpensesRepository> _repository;

    public ExpensesRepositoryBuilder()
    {
        _repository = new Mock<IExpensesRepository>();
    }

    public IExpensesRepository Build() => _repository.Object;

    public ExpensesRepositoryBuilder GetAllRead(User user, List<Expense> expenses)
    {
        _repository.Setup(x => x.GetAll(user)).ReturnsAsync(expenses);
        return this;
    }

    public ExpensesRepositoryBuilder GetById(Guid userId, Expense? expense)
    {
        if (expense is not null) _repository.Setup(x => x.GetByIdRead(expense.Id, userId)).ReturnsAsync(expense);
        return this;
    }

    public ExpensesRepositoryBuilder DeleteById(Expense? expense)
    {
        if (expense is not null) _repository.Setup(x => x.DeleteById(expense.Id)).ReturnsAsync(true);
        return this;
    }

    public ExpensesRepositoryBuilder UpdateById(Expense? expense)
    {
        if (expense is not null) _repository.Setup(x => x.UpdateById(expense)).ReturnsAsync(true);
        return this;
    }

    public ExpensesRepositoryBuilder GetByIdUpdate(long? id, Guid userId, Expense? expense)
    {
        if ((expense is not null) && (id is not null)) _repository.Setup(x => x.GetByIdUpdate((long) id, userId)).ReturnsAsync(expense);
        return this;
    }

    public ExpensesRepositoryBuilder GetByIdMonth(Guid userId, List<Expense> expenses)
    {
        _repository.Setup(x => x.FilterByMonth(It.IsAny<DateOnly>(), userId)).ReturnsAsync(expenses);
        return this;
    }
}
