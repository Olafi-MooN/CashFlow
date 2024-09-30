using CashFlow.Application.DTOs;
using CashFlow.Application.Interfaces;
using CashFlow.Communication;

namespace CashFlow.Application.UseCases.Users.Delete;
public interface IDeleteUserUseCase : IUseCase<RequestUpdateUserJson, Task<VoidResult>>
{
}
