namespace CashFlow.Domain;

public interface IExpensesWriteOnlyRepository
{
    Task Add(Expense expense);

    /// <summary>
    /// This method is used to delete an expense by id and return TRUE if the expense was found and deleted.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteById(long id);
}
