using System.Text.RegularExpressions;
using CashFlow.Domain.Messages.Reports;
using FluentValidation;
using FluentValidation.Validators;

namespace CashFlow.Application;

public partial class PasswordValidator<T> : PropertyValidator<T, string>
{
    private const string ERROR_MESSAGE_KEY = "ErrorMessage";
    public override string Name => "PasswordValidator";

    [GeneratedRegex(@"[A-Z]+")]
    private static partial Regex UpperCaseLetter();
    [GeneratedRegex(@"[a-z]+")]
    private static partial Regex LowerCaseLetter();
    [GeneratedRegex(@"[0-9]+")]
    private static partial Regex Digits();
    [GeneratedRegex(@"[\!\?\*\.]+")]
    private static partial Regex EspecialSymbols();

    protected override string GetDefaultMessageTemplate(string errorCode) => $"{{{ERROR_MESSAGE_KEY}}}";
    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceReportGenerationMessages.PASSWORD_REQUIRED);
            return false;
        }

        if (password.Length < 8)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceReportGenerationMessages.PASSWORD_REQUIRED);
            return false;
        }

        if (!UpperCaseLetter().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceReportGenerationMessages.PASSWORD_REQUIRED);
            return false;
        }

        if (!LowerCaseLetter().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceReportGenerationMessages.PASSWORD_REQUIRED);
            return false;
        }

        if (!Digits().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceReportGenerationMessages.PASSWORD_REQUIRED);
            return false;
        }

        if (!EspecialSymbols().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceReportGenerationMessages.PASSWORD_REQUIRED);
            return false;
        }

        return true;
    }

}
