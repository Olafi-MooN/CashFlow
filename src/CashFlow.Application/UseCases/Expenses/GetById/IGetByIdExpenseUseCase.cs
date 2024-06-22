using CashFlow.Application.Interfaces;
using CashFlow.Communication.Responses;

namespace CashFlow.Application;

public interface IGetByIdExpenseUseCase : IUseCase<long, Task<ResponseExpenseJson>>
{

}
