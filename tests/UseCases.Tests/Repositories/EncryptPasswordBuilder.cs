using CashFlow.Domain;
using Moq;

namespace UseCases.Tests;

public class EncryptPasswordBuilder
{
    private readonly Mock<IEncryptPassword> _mock;
    public EncryptPasswordBuilder()
    {
        _mock = new Mock<IEncryptPassword>();
        _mock.Setup(mock => mock.Encrypt(It.IsAny<string>())).Returns("$#%Fvxvbcvbjfhjjhffgh");
    }

    public IEncryptPassword Build() => _mock.Object;

    public EncryptPasswordBuilder Verify(string? password)
    {
        if (!string.IsNullOrWhiteSpace(password)) _mock.Setup(mock => mock.Verify(password, It.IsAny<string>())).Returns(true);
        return this;
    }
}
