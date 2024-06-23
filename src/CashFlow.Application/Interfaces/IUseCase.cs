

namespace CashFlow.Application.Interfaces;

public interface IUseCase<in TRequest, out TResponse>
{
    TResponse Execute(TRequest request = default!);

}