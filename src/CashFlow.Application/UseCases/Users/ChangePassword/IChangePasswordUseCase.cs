using CashFlow.Application.DTOs;
using CashFlow.Application.Interfaces;
using CashFlow.Communication;

namespace CashFlow.Application.UseCases.Users.ChangePassword;
public interface IChangePasswordUseCase : IUseCase<RequestChangePasswordJson, Task<VoidResult>>
{
}
