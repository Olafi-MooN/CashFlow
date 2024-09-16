using System;
using CashFlow.Domain;

namespace WebApi.Tests.Resources;

public class UserIdentityManager
{
    private readonly string _token;
    private readonly User _user;
    private readonly string _password;


    public UserIdentityManager(User user, string password, string token)
    {
        _user = user;
        _token = token;
        _password = password;
    }

    public User GetUser() => _user;
    public string GetToken() => _token;
    public string GetPassword() => _password;
}
