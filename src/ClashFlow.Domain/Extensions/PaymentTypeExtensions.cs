using CashFlow.Domain.Messages.Reports;

namespace CashFlow.Domain;

public static class PaymentTypeExtensions
{
    public static string PaymentTypeToString(this EPaymentTypeEnum paymentType)
    {
        return paymentType switch
        {
            EPaymentTypeEnum.Cash => ResourceReportGenerationMessages.CASH,
            EPaymentTypeEnum.CreditCard => ResourceReportGenerationMessages.CREDIT_CARD,
            EPaymentTypeEnum.DebitCard => ResourceReportGenerationMessages.DEBIT_CARD,
            EPaymentTypeEnum.EletronicTransferer => ResourceReportGenerationMessages.ELETRONIC_TRANSFER,
            _ => string.Empty
        };
    }
}