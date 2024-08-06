using System.Net;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Exception;

public class InvalidLoginException : CashFlowException
{
    public override int StatusCode => (int)HttpStatusCode.Unauthorized;
    public InvalidLoginException() : base(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID) { }

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
