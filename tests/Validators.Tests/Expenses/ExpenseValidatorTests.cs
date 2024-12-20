﻿using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Expenses;

public class ExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        // Arrange - Configuração da instancias do que precisa ser executado o teste
        var validator = new ExpensiveValidator();
        var request = new RequestRegisterExpensiveJsonBuilder().Build();

        // Act 
        var result = validator.Validate(request);

        // Assert - Qual resultado é esperado?
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ErrorTitleEmpty(string? title)
    {
        // Arrange - Configuração da instancias do que precisa ser executado o teste
        var validator = new ExpensiveValidator();
        var request = new RequestRegisterExpensiveJsonBuilder().Build();
        request.Title = title ?? string.Empty;

        // Act 
        var result = validator.Validate(request);

        // Assert - Qual resultado é esperado?
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Fact]
    public void ErrorPaymentTypeInvalid()
    {
        // Arrange - Configuração da instancias do que precisa ser executado o teste
        var validator = new ExpensiveValidator();
        var request = new RequestRegisterExpensiveJsonBuilder().Build();
        request.PaymentType = (EPaymentTypeEnum) 1000;

        // Act 
        var result = validator.Validate(request);

        // Assert - Qual resultado é esperado?
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_ENUM));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-5)]
    public void ErrorAmoutInvalid(decimal amount)
    {
        // Arrange - Configuração da instancias do que precisa ser executado o teste
        var validator = new ExpensiveValidator();
        var request = new RequestRegisterExpensiveJsonBuilder().Build();
        request.Amount = amount;

        // Act 
        var result = validator.Validate(request);

        // Assert - Qual resultado é esperado?
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_GREATER_THAN_ZERO));
    }

    [Fact]
    public void Error_Tag_Invalid()
    {
        // Arrange - Configuração da instancias do que precisa ser executado o teste
        var validator = new ExpensiveValidator();
        var request = new RequestRegisterExpensiveJsonBuilder().Build();
        var enumValues = Enum.GetValues(typeof(ETagTypeEnum)).Cast<ETagTypeEnum>();
        var enumNamesWithValues = string.Join(", ", enumValues.Select(v => $"{v}={(int) v}"));
        request.Tags.Add((ETagTypeEnum) 1000);

        // Act 
        var result = validator.Validate(request);

        // Assert - Qual resultado é esperado?
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(string.Format(ResourceErrorMessages.MUST_BE_IN_ENUM, nameof(ETagTypeEnum), enumNamesWithValues)));
    }
}
