
using CashFlow.Application.DTOs;
using CashFlow.Application.Interfaces;
using CashFlow.Communication;

namespace CashFlow.Application.UseCases.Users.Profile;
public interface IGetUserProfileUseCase : IUseCase<VoidResult, Task<ResponseUserProfileJson>>
{

}
