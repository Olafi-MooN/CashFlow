using System;
using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace WebApi.Tests.Expenses.GetAll;

public class GetAllExpenseTest : CashFlowClassFixture
{
    private const string REQUEST_URI = "/api/expenses";
    private readonly string _token;

    public GetAllExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(REQUEST_URI, _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();
        var bodyJson = await JsonDocument.ParseAsync(body);
        bodyJson.RootElement.GetProperty("expenses").EnumerateArray().Should().NotBeNullOrEmpty();

    }
}
