using CashFlow.Communication.Requests;

namespace CashFlow.Communication;

public class RequestUpdateExpenseJson : RequestRegisterExpensiveJson
{
    public long RouteId { get; set; }
}
