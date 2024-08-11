using AutoMapper;
using CashFlow.Application;

namespace UseCases.Tests;

public static class MapperBuilder
{
    public static IMapper Build()
    {
        var mapper = new MapperConfiguration(config =>
        {
            config.AddProfile(new AutoMapping());
        });

        return mapper.CreateMapper();
    }
}
