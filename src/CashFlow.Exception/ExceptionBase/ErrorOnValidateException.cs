namespace CashFlow.Exception.ExceptionBase;

public class ErrorOnValidateException : CashFlowException
{
    public List<string> Errors { get; set; }

    public ErrorOnValidateException(List<string> errorMessages)
    {
        Errors = errorMessages;
    }
}
