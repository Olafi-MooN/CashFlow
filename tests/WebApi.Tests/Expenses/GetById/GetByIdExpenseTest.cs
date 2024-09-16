using System;
using System.Net;
using System.Text.Json;
using CashFlow.Domain;
using FluentAssertions;

namespace WebApi.Tests.Expenses.GetById;

public class GetByIdExpenseTest : CashFlowClassFixture
{
    private readonly string _token;
    private readonly Expense _expense;
    private const string REQUEST_URI = "api/expenses";


    public GetByIdExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
        _expense = webApplicationFactory.ExpenseManager.GetExpense();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoGet($"{REQUEST_URI}/{_expense.Id}", _token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadAsStreamAsync();

        body.Should().NotBeNull();

        var bodyJson = await JsonDocument.ParseAsync(body);

        bodyJson.RootElement.GetProperty("title").GetString().Should().Be(_expense.Title);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-5)]
    public async Task Error_Expense_Not_Found(int id)
    {
        var response = await DoGet($"{REQUEST_URI}/{id}", _token);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


}
