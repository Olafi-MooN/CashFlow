using CashFlow.Application.Interfaces;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application;

public interface IRegisterExpenseUseCase : IUseCase<RequestRegisterExpensiveJson, Task<ResponseRegisteredExpenseJson>> { }