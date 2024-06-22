using CashFlow.Communication.Responses;

namespace CashFlow.Communication.Responses;

public class ResponseExpensesJson
{
    public List<ResponseExpenseJson> Expenses { get; set; } = [];
}
