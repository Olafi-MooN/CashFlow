using CashFlow.Application.Interfaces;

namespace CashFlow.Application;

public interface IDeleteExpenseByIdUseCase : IUseCase<long, Task<bool>>
{

}
