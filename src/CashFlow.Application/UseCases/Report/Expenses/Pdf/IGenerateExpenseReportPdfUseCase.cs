using CashFlow.Application.Interfaces;
using CashFlow.Communication;

namespace CashFlow.Application;

public interface IGenerateExpenseReportPdfUseCase : IUseCase<RequestInFormationReportJson, Task<byte[]>>
{

}
