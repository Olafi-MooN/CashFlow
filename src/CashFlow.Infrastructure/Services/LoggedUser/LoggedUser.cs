using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CashFlow.Domain;
using CashFlow.Domain.Security.token;
using CashFlow.Domain.Services.LoggedUser;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly CashFlowDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;


    public LoggedUser(CashFlowDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }
    public Task<User> Get()
    {
        string token = _tokenProvider.TokenOnRequest();

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityTokenHandler = tokenHandler.ReadJwtToken(token);
        var userId = jwtSecurityTokenHandler.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

        return _dbContext.Users.AsNoTracking().FirstAsync(user => user.Id == Guid.Parse(userId));
    }
}
