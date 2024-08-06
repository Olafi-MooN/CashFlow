namespace CashFlow.Domain;

public interface IEncryptPassword
{
    string Encrypt(string password);
    bool Verify(string password, string hash);
}
