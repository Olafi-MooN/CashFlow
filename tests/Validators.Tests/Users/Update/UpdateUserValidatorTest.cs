using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Domain.Messages.Reports;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Users.Update;
public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateUserValidator();
        var user = RequestUpdateUserJsonFakerBuilder.Build();

        var result = validator.Validate(user);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name)
    {
        var validator = new UpdateUserValidator();
        var user = RequestUpdateUserJsonFakerBuilder.Build();
        user.Name = name;

        var result = validator.Validate(user);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceReportGenerationMessages.NAME_REQUIRED));
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        var validator = new UpdateUserValidator();
        var user = RequestUpdateUserJsonFakerBuilder.Build();
        user.Email = email;

        var result = validator.Validate(user);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceReportGenerationMessages.EMAIL_REQUIRED));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new UpdateUserValidator();
        var user = RequestUpdateUserJsonFakerBuilder.Build();
        user.Email = "email.com";

        var result = validator.Validate(user);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceReportGenerationMessages.EMAIL_REQUIRED));
    }
}
