using System;
using Bogus;
using CashFlow.Domain;
using Microsoft.VisualBasic;

namespace CommonTestUtilities.Entities;

public static class ExpenseBuilder
{
    public static Expense Build(User user)
    {
        return new Faker<Expense>()
            .RuleFor(expense => expense.Id, faker => 1)
            .RuleFor(expense => expense.Title, faker => faker.Commerce.ProductName())
            .RuleFor(expense => expense.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(expense => expense.Date, faker => faker.Date.Past())
            .RuleFor(expense => expense.PaymentType, faker => faker.PickRandom<EPaymentTypeEnum>())
            .RuleFor(expense => expense.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
            .RuleFor(expense => expense.UserId, _ => user.Id);
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
