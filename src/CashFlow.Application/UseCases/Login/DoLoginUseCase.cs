using CashFlow.Application.DTOs;
using CashFlow.Communication;
using CashFlow.Domain;
using CashFlow.Exception;

namespace CashFlow.Application;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUsersRepository _repository;
    private readonly IEncryptPassword _encryptPassword;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public DoLoginUseCase(IUsersRepository repository, IEncryptPassword encryptPassword, IAccessTokenGenerator accessTokenGenerator)
    {
        _repository = repository;
        _encryptPassword = encryptPassword;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request = default!)
    {
        var user = await _repository.GetByEmail(request.Email) ?? throw new InvalidLoginException();
        if (!_encryptPassword.Verify(request.Password, user.Password)) throw new InvalidLoginException();

        return new ResponseRegisteredUserJson()
        {
            Id = user.Id,
            Token = _accessTokenGenerator.Generate(user)
        };
    }
}
