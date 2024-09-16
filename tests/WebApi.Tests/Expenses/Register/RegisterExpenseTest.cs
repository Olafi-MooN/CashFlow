using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.Expenses.Register;

public class RegisterExpenseTest : CashFlowClassFixture
{
    private const string METHOD = "api/expenses";

    private readonly string _token;

    public RegisterExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestRegisterExpensiveJsonBuilder().Build();

        var response = await DoPost(METHOD, request, _token);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadAsStreamAsync();

        body.Should().NotBeNull();

        var bodyJson = await JsonDocument.ParseAsync(body);
        bodyJson.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Title_Name(string cultureName)
    {
        var request = new RequestRegisterExpensiveJsonBuilder().Build();
        request.Title = string.Empty;

        var response = await DoPost(METHOD, request, _token, cultureName);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStreamAsync();

        body.Should().NotBeNull();

        var bodyJson = await JsonDocument.ParseAsync(body);

        bodyJson.RootElement.GetProperty("errorMessages")
            .EnumerateArray().Should().HaveCount(1).And
            .Contain(errors => errors.GetString()!.Equals(ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(cultureName))));
    }

    [Fact]
    public async Task Error_Token_Empty()
    {
        var request = new RequestRegisterExpensiveJsonBuilder().Build();

        var response = await DoPost(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
