using CashFlow.Application.Interfaces;
using CashFlow.Communication;

namespace CashFlow.Application;

public interface IGenerateExpenseReportExcelUseCase : IUseCase<RequestInFormationReportJson, Task<byte[]>>
{

}
