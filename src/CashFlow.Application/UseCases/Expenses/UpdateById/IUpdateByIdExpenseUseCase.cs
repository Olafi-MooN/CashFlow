using CashFlow.Application.Interfaces;
using CashFlow.Communication;
using CashFlow.Communication.Responses;

namespace CashFlow.Application;

public interface IUpdateByIdExpenseUseCase : IUseCase<RequestUpdateExpenseJson, Task<ResponseExpenseJson>>
{

}
