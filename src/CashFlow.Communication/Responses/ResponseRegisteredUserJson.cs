namespace CashFlow.Communication;

public class ResponseRegisteredUserJson
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Token { get; set; } = string.Empty;
}
