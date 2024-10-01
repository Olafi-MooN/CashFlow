using Bogus;
using CashFlow.Domain;

namespace CommonTestUtilities.Entities;

public static class ExpenseBuilder
{
    public static Expense Build(User user, long expenseId = 1)
    {
        return new Faker<Expense>()
            .RuleFor(expense => expense.Id, _ => expenseId)
            .RuleFor(expense => expense.Title, faker => faker.Commerce.ProductName())
            .RuleFor(expense => expense.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(expense => expense.Date, faker => new DateTime(2024, 09, 30))
            .RuleFor(expense => expense.PaymentType, faker => faker.PickRandom<EPaymentTypeEnum>())
            .RuleFor(expense => expense.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
            .RuleFor(expense => expense.UserId, _ => user.Id)
            .RuleFor(r => r.Tags, faker => faker.Make(1, () => new CashFlow.Domain.Entities.Tag
            {
                Id = faker.Random.Int(),
                Value = faker.PickRandom<CashFlow.Domain.Enums.ETagTypeEnum>(),
                ExpenseId = expenseId,
            }));
    }

    public static List<Expense> Collection(User user, uint count = 2)
    {
        var expenses = new List<Expense>();
        var auxExpenseId = 1;
        if (count == 0) return expenses;

        for (int i = 0; i < count; i++)
        {
            var expense = Build(user);
            expense.Id = auxExpenseId++;
            expenses.Add(expense);
        }

        return expenses;
    }
}
