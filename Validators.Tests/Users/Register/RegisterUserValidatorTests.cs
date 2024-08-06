using CashFlow.Application;
using CashFlow.Domain.Messages.Reports;
using CommonTestUtilities;
using FluentAssertions;

namespace Validators.Tests.Users.Register;

public class RegisterUserValidatorTests
{

    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new UserValidator();
        var request = RequestsRegisterUserJsonBuilder.Build();

        // Act
        var result = validator.Validate(request);
        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(null)]
    public void Error_Name_Empty(string? name)
    {
        var validator = new UserValidator();
        var request = RequestsRegisterUserJsonBuilder.Build();
        request.Name = name!;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceReportGenerationMessages.NAME_REQUIRED));
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(null)]
    public void Error_Email_Empty(string? email)
    {
        var validator = new UserValidator();
        var request = RequestsRegisterUserJsonBuilder.Build();
        request.Email = email!;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceReportGenerationMessages.EMAIL_REQUIRED));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new UserValidator();
        var request = RequestsRegisterUserJsonBuilder.Build();
        request.Email = "alef.com";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceReportGenerationMessages.EMAIL_REQUIRED));
    }

    [Fact]
    public void Error_Password_Empty()
    {
        var validator = new UserValidator();
        var request = RequestsRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceReportGenerationMessages.PASSWORD_REQUIRED));
    }


}
