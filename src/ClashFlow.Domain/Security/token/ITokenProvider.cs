namespace CashFlow.Domain.Security.token;

public interface ITokenProvider
{
    string TokenOnRequest();
}
