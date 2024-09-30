using CashFlow.Application.DTOs;
using CashFlow.Communication;
using CashFlow.Domain;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.ChangePassword;
public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUsersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEncryptPassword _encryptPassword;

    public ChangePasswordUseCase(ILoggedUser loggedUser, IUsersRepository repository, IUnitOfWork unitOfWork, IEncryptPassword encryptPassword)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _encryptPassword = encryptPassword;
    }

    public async Task<VoidResult> Execute(RequestChangePasswordJson request = default!)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(loggedUser, request);

        var user = (await _repository.GetById(loggedUser.Id))!;
        user.Password = _encryptPassword.Encrypt(request.NewPassword);

        _repository.Update(user);

        await _unitOfWork.Commit();

        return new VoidResult();
    }

    private void Validate(User loggedUser, RequestChangePasswordJson request = default!)
    {
        var validator = new ChangePasswordValidator();

        var result = validator.Validate(request);

        var passwordMatch = _encryptPassword.Verify(request.OldPassword, loggedUser.Password);

        if (!passwordMatch) result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

        if (!result.IsValid)
        {
            var messagesError = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ErrorOnValidateException(messagesError);
        }
    }
}
