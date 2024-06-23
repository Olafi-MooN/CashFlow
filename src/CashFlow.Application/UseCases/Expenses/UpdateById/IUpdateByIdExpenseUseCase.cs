using CashFlow.Application.Interfaces;
using CashFlow.Communication;

namespace CashFlow.Application;

public interface IUpdateByIdExpenseUseCase : IUseCase<RequestUpdateExpenseJson, Task<object>>
{

}
