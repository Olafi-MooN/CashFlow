using CashFlow.Domain;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infrastructure;

public class BCrypt : IEncryptPassword
{
    public string Encrypt(string password)
    {
        return BC.HashPassword(password);
    }
}
