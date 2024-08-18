using CashFlow.Infrastructure;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using CashFlow.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly User _user = UserBuilder.Build();
    private string _userPassword = string.Empty;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test").ConfigureServices(services =>
        {
            var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            services.AddDbContext<CashFlowDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

            var encryptPassword = services.BuildServiceProvider().GetRequiredService<IEncryptPassword>();
            var dbContext = services.BuildServiceProvider().GetRequiredService<CashFlowDbContext>();

            StartDatabase(dbContext, encryptPassword);
        });
    }

    public User GetUser() => _user;
    public string GetUserPassword() => _userPassword;

    private void StartDatabase(CashFlowDbContext dbContext, IEncryptPassword encryptPassword)
    {
        _userPassword = _user.Password;
        _user.Password = encryptPassword.Encrypt(_user.Password);
        dbContext.Users.Add(_user);
        dbContext.SaveChanges();
    }
}
