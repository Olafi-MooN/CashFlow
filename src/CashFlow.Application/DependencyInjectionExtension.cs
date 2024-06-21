using Microsoft.Extensions.DependencyInjection;
using CashFlow.Application.Interfaces;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Responses;
using CashFlow.Communication.Requests;

namespace CashFlow.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUseCase<RequestRegisterExpensiveJson, Task<ResponseRegisteredExpenseJson>>, RegisterExpensiveUseCase>();
    }
}
