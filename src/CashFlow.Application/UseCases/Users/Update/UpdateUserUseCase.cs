using CashFlow.Application.DTOs;
using CashFlow.Communication;
using CashFlow.Domain;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Update;
public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly IUsersRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserUseCase(IUsersRepository userRepository, ILoggedUser loggedUser, IUnitOfWork unitOfWork)
    {
        _repository = userRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<VoidResult> Execute(RequestUpdateUserJson request = default!)
    {
        var loggedUser = await _loggedUser.Get();

        var user = (await _repository.GetById(loggedUser.Id))!;

        await Validate(request, loggedUser.Email);

        user.Name = request.Name;
        user.Email = request.Email;

        _repository.Update(user);

        await _unitOfWork.Commit();

        return new VoidResult();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validator = new UpdateUserValidator();

        var result = validator.Validate(request);

        if (!currentEmail.Equals(request.Email))
        {
            var userExist = await _repository.ExistActiveUserWithEmail(request.Email);
            if (userExist) result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if (!result.IsValid)
        {
            var messagesError = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ErrorOnValidateException(messagesError);
        }
    }
}
