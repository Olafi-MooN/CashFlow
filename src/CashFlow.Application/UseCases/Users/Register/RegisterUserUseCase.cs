using AutoMapper;
using CashFlow.Communication;
using CashFlow.Domain;
using CashFlow.Domain.Messages.Reports;
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

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request = default!)
    {
        await Validate(request);

        var userRequest = _mapper.Map<User>(request);
        userRequest.UserIdentifier = Guid.NewGuid();
        userRequest.Password = _encryptPassword.Encrypt(userRequest.Password);

        var userEntity = await _repository.Add(userRequest);
        var userResponseRegisteredUser = _mapper.Map<ResponseRegisteredUserJson>(userEntity);

        await _unitOfWork.Commit();

        return userResponseRegisteredUser;
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var messagesError = new UserValidator().Validate(request).Errors.Select(x => x.ErrorMessage).ToList();
        var existActiveUserWithEmail = await _repository.ExistActiveUserWithEmail(request.Email);

        if (existActiveUserWithEmail) messagesError.Add(ResourceReportGenerationMessages.USER_ALREADY_EXISTS);

        if (messagesError.Count > 0) throw new ErrorOnValidateException(messagesError);
    }
}
