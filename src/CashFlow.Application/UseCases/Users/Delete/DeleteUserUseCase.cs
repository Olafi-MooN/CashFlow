using CashFlow.Application.DTOs;
using CashFlow.Communication;
using CashFlow.Domain;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Users.Delete;
public class DeleteUserUseCase : IDeleteUserUseCase
{
    private readonly IUsersRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserUseCase(IUsersRepository userRepository, ILoggedUser loggedUser, IUnitOfWork unitOfWork)
    {
        _repository = userRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<VoidResult> Execute(RequestUpdateUserJson request = default!)
    {
        var loggedUser = await _loggedUser.Get();

        await _repository.Delete(loggedUser.Id);

        await _unitOfWork.Commit();

        return new VoidResult();
    }
}
