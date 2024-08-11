using CashFlow.Domain;
using Moq;

namespace UseCases.Tests;

public static class AccessTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();
        mock.Setup(m => m.Generate(It.IsAny<User>())).Returns("dummy_token"); // Adicione um valor de retorno
        return mock.Object;
    }
}
