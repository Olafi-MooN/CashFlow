namespace CashFlow.Exception.ExceptionBase;

public abstract class CashFlowException : SystemException
{
    public CashFlowException(string? message) : base(message)
    {
    }
}
