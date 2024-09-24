using CashFlow.Domain;
using Moq;

namespace CommonTestUtilities.Cryptography;

public static class EncryptPasswordBuilder
{
    public static IEncryptPassword Build()
    {
        var mock = new Mock<IEncryptPassword>();
        mock.Setup(mock => mock.Encrypt(It.IsAny<string>())).Returns("$#%Fvxvbcvbjfhjjhffgh");
        mock.Setup(mock => mock.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        return mock.Object;
    }
}