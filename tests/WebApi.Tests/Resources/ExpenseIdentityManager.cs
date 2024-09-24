using CashFlow.Domain;

namespace WebApi.Tests.Resources;

public class ExpenseIdentityManager
{
    public readonly Expense _expenses;
    public ExpenseIdentityManager(Expense expense)
    {
        _expenses = expense;
    }

    public Expense GetExpense() => _expenses;
}
