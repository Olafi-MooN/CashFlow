namespace CashFlow.Domain;

public class Expense
{
  public long Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public DateTime Date { get; set; }
  public string Description { get; set; } = string.Empty;
  public decimal Amount { get; set; }
  public EPaymentTypeEnum PaymentType { get; set; }
  public Guid UserId { get; set; }
  public User User { get; set; } = new();
}
