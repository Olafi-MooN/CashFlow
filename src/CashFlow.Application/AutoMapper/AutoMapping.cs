using AutoMapper;
using CashFlow.Communication;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain;
using CashFlow.Domain.Entities;

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

        CreateMap<RequestRegisterExpensiveJson, Expense>().
            ForMember(dest => dest.Tags, config => config.MapFrom(source => source.Tags.Distinct()));

        CreateMap<Communication.Enums.ETagTypeEnum, Tag>().
            ForMember(dest => dest.Value, config => config.MapFrom(source => source));
    }

    private void EntityToResponseAndReverse()
    {
        CreateMap<Expense, ResponseExpenseJson>()
            .ForMember(dest => dest.Tags, config => config.MapFrom(source => source.Tags.Select(tag => tag.Value)))
            .ReverseMap();

        CreateMap<Expense, ResponseRegisteredExpenseJson>().ReverseMap();
        CreateMap<List<Expense>, ResponseExpensesJson>().ReverseMap();
        CreateMap<User, ResponseRegisteredUserJson>().ReverseMap();
        CreateMap<User, ResponseUserProfileJson>().ReverseMap();

    }
}
