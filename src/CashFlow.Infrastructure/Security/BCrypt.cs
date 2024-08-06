using CashFlow.Domain;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infrastructure;

public class BCrypt : IEncryptPassword
{
    public string Encrypt(string password) => BC.HashPassword(password);
    public bool Verify(string password, string hash) => BC.Verify(password, hash);

}
