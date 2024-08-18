

namespace CashFlow.Application.Interfaces;

public interface IUseCase<in TRequest, out TResponse>
{
    TResponse Execute(TRequest request = default!);
    TResponse? Execute(TRequest request = default!, params object[] parameters)
    {
        return default;
    }
}