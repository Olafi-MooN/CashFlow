using CashFlow.Domain.Security.token;
using CashFlow.Exception;

namespace CashFlow.Api.Token;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string TokenOnRequest()
    {
        var authorization = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString() ?? "";

        if (string.IsNullOrEmpty(authorization)) throw new UnauthorizedAccessException(ResourceErrorMessages.TOKEN_REQUIRED);

        var token = authorization["Bearer ".Length..];

        return token;
    }
}
