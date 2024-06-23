using AutoMapper;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication;
using CashFlow.Communication.Responses;
using CashFlow.Domain;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application;

public class UpdateByIdExpenseUseCase : IUpdateByIdExpenseUseCase
{
    private readonly IExpensesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateByIdExpenseUseCase(
        IExpensesRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseExpenseJson> Execute(RequestUpdateExpenseJson request)
    {
        Validate(request);
        var expense = await (_repository as IExpensesUpdateOnlyRepository).GetById(request.RouteId) ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        await _repository.UpdateById(_mapper.Map(request, expense));
        await _unitOfWork.Commit();

        return _mapper.Map<ResponseExpenseJson>(expense);
    }

    private void Validate(RequestUpdateExpenseJson request)
    {
        var messagesError = new ExpensiveValidator().Validate(request).Errors.Select(x => x.ErrorMessage).ToList();
        if (messagesError.Count > 0)
            throw new ErrorOnValidateException(messagesError);
    }
}
