using AutoMapper;
using CashFlow.Communication;
using CashFlow.Domain;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsersRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEncryptPassword _encryptPassword;

    public RegisterUserUseCase(IUnitOfWork unitOfWork, IUsersRepository repository, IMapper mapper, IEncryptPassword encryptPassword)
    {
        _mapper = mapper;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _encryptPassword = encryptPassword;
    }

    public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request = default!)
    {
        Validate(request);

        var userRequest = _mapper.Map<User>(request);
        userRequest.Password = _encryptPassword.Encrypt(userRequest.Password);

        var userEntity = _repository.Add(userRequest);
        var userResponseRegisteredUser = _mapper.Map<ResponseRegisteredUserJson>(userEntity);

        _unitOfWork.Commit();

        return Task.FromResult(userResponseRegisteredUser);
    }

    private static void Validate(RequestRegisterUserJson request)
    {
        var messagesError = new UserValidator().Validate(request).Errors.Select(x => x.ErrorMessage).ToList();
        if (messagesError.Count > 0) throw new ErrorOnValidateException(messagesError);
    }
}
