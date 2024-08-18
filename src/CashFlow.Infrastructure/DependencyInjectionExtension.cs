using CashFlow.Domain;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Infrastructure.Extensions;
using CashFlow.Infrastructure.Services.LoggedUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddToken(services, configuration);
        if (!configuration.IsTestEnvironment()) AddDbContext(services, configuration);
        AddRepositories(services);
        AddSecurity(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IExpensesRepository, ExpensesRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddSecurity(IServiceCollection services)
    {
        services.AddScoped<IEncryptPassword, BCrypt>();
        services.AddScoped<ILoggedUser, LoggedUser>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        services.AddDbContext<CashFlowDbContext>(config => config.UseMySql(connectionString, serverVersion));
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var signinKey = configuration.GetValue<string>("Settings:Jwt:SigninKey");
        var expiresMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");

        services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expiresMinutes, signinKey!));
    }
}
