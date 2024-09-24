using CashFlow.Infrastructure;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using CashFlow.Domain;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Tests.Resources;
using PdfSharp.Drawing;

namespace WebApi.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public ExpenseIdentityManager ExpenseTeamMemberManager { get; private set; } = default!;
    public ExpenseIdentityManager ExpenseAdminManager { get; private set; } = default!;
    public UserIdentityManager UserTeamMember { get; private set; } = default!;
    public UserIdentityManager UserAdmin { get; private set; } = default!;

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

            var scope = services.BuildServiceProvider();
            var encryptPassword = scope.GetRequiredService<IEncryptPassword>();
            var dbContext = scope.GetRequiredService<CashFlowDbContext>();
            var tokenGenerator = scope.GetRequiredService<IAccessTokenGenerator>();

            StartDatabase(dbContext, encryptPassword, tokenGenerator);
        });
    }

    private void StartDatabase(CashFlowDbContext dbContext, IEncryptPassword encryptPassword, IAccessTokenGenerator tokenGenerator)
    {
        var userTeamMember = AddUserTeamMember(dbContext, encryptPassword, tokenGenerator);
        var userAdmin = AddUserAdmin(dbContext, encryptPassword, tokenGenerator);

        ExpenseTeamMemberManager = new ExpenseIdentityManager(AddExpenses(dbContext, userTeamMember, expenseId: 1));
        ExpenseAdminManager = new ExpenseIdentityManager(AddExpenses(dbContext, userAdmin, expenseId: 2));

        dbContext.SaveChanges();
    }

    private User AddUserTeamMember(CashFlowDbContext dbContext, IEncryptPassword encryptPassword, IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build();
        var password = user.Password;
        user.Password = encryptPassword.Encrypt(user.Password);
        dbContext.Users.Add(user);
        var token = tokenGenerator.Generate(user);
        UserTeamMember = new UserIdentityManager(user, password, token);
        return user!;
    }

    private User AddUserAdmin(CashFlowDbContext dbContext, IEncryptPassword encryptPassword, IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        var password = user.Password;
        user.Password = encryptPassword.Encrypt(user.Password);
        dbContext.Users.Add(user);
        var token = tokenGenerator.Generate(user);
        UserAdmin = new UserIdentityManager(user, password, token);
        return user!;
    }

    private static Expense AddExpenses(CashFlowDbContext dbContext, User user, long expenseId)
    {
        var expense = ExpenseBuilder.Build(user, expenseId);
        dbContext.Expenses.Add(expense);

        return expense;
    }
}
