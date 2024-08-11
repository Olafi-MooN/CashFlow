using AutoMapper;
using CashFlow.Communication;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain;

namespace CashFlow.Application;

public class AutoMapping : Profile
{

    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponseAndReverse();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestRegisterExpensiveJson, Expense>().ReverseMap();
        CreateMap<RequestRegisterUserJson, User>().ForMember(x => x.Password, x => x.Ignore()).ReverseMap();
        CreateMap<RequestLoginJson, User>().ReverseMap();
    }

    private void EntityToResponseAndReverse()
    {
        CreateMap<Expense, ResponseRegisteredExpenseJson>().ReverseMap();
        CreateMap<Expense, ResponseExpenseJson>().ReverseMap();
        CreateMap<List<Expense>, ResponseExpensesJson>().ReverseMap();
        CreateMap<User, ResponseRegisteredUserJson>().ReverseMap();
    }
}
