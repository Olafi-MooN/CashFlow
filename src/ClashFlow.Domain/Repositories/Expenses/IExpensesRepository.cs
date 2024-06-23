namespace CashFlow.Domain;

public interface IExpensesRepository : IExpenseReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository { }
