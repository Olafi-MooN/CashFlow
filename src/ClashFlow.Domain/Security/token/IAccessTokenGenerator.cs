namespace CashFlow.Domain;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}
