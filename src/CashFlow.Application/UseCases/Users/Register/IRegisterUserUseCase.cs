using CashFlow.Application.DTOs;
using CashFlow.Application.Interfaces;
using CashFlow.Communication;

namespace CashFlow.Application;

public interface IRegisterUserUseCase : IUseCase<RequestRegisterUserJson, Task<ResponseRegisteredUserJson>>
{

}
