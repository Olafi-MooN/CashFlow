using CashFlow.Domain;
using Moq;

namespace UseCases.Tests;

public static class UnitOfWorkBuilder
{
    public static IUnitOfWork Build() => new Mock<IUnitOfWork>().Object;
}
