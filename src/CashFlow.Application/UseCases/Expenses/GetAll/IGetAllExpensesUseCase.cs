using CashFlow.Application.Interfaces;
using CashFlow.Communication.Responses;

namespace CashFlow.Application;

public interface IGetAllExpensesUseCase : IUseCase<object, Task<ResponseExpensesJson>> { }
