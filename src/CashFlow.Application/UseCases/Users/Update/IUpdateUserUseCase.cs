using CashFlow.Application.DTOs;
using CashFlow.Application.Interfaces;
using CashFlow.Communication;

namespace CashFlow.Application.UseCases.Users.Update;
public interface IUpdateUserUseCase : IUseCase<RequestUpdateUserJson, Task<VoidResult>>
{
}
