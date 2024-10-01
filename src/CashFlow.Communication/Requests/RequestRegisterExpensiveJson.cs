using CashFlow.Communication.Enums;

namespace CashFlow.Communication.Requests;

public class RequestRegisterExpensiveJson
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public EPaymentTypeEnum PaymentType { get; set; } = EPaymentTypeEnum.DebitCard;
    public IList<ETagTypeEnum> Tags { get; set; } = [];
}
