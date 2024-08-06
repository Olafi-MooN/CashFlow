using CashFlow.Application;
using CashFlow.Communication;
using FluentAssertions;
using FluentValidation;

namespace Validators.Tests.Users;

public class PasswordValidatorTest
{
    [Theory]
    [InlineData("_Senha123!")]
    [InlineData("_Senha!212")]
    [InlineData("Senha_!212")]
    [InlineData("Se_!212")]
    [InlineData("Senha_!2#")]
    public void Success(string password)
    {
        // Arrange
        var validator = new PasswordValidator<RequestRegisterUserJson>();

        // Act
        var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData("123")]
    [InlineData("1234")]
    [InlineData("AAAA")]
    [InlineData("AaaBB34")]
    [InlineData("AaaBB_!")]
    public void Error_Password_Is_Invalid(string password)
    {
        // Arrange
        var validator = new PasswordValidator<RequestRegisterUserJson>();

        // Act
        var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password);

        // Assert
        result.Should().BeFalse();
    }
}
