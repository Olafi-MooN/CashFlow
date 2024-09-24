using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Domain;
using CashFlow.Exception;
using FluentAssertions;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.Expenses.DeleteById;

public class DeleteByIdExpenseTest : CashFlowClassFixture
{
    private const string REQUEST_URI = "/api/expenses";
    private readonly string _token;
    private readonly Expense _expense;

    public DeleteByIdExpenseTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
    {
        _token = customWebApplicationFactory.UserTeamMember.GetToken();
        _expense = customWebApplicationFactory.ExpenseTeamMemberManager.GetExpense();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoDelete($"{REQUEST_URI}/{_expense.Id}", _token);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        response = await DoGet($"{REQUEST_URI}/{_expense.Id}", _token);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Expense_Not_Found(string cultureName)
    {
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(cultureName));

        var response = await DoDelete($"{REQUEST_URI}/{-1}", _token, cultureName);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await response.Content.ReadAsStreamAsync();
        body.Should().NotBeNull();

        var bodyJson = await JsonDocument.ParseAsync(body);

        var errors = bodyJson.RootElement.GetProperty("errorMessages").EnumerateArray();
        errors.Should().HaveCount(1).And.Contain(errors => errors.GetString()!.Equals(expectedMessage));
    }
}
