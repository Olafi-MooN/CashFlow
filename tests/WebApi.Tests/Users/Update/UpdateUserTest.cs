using CashFlow.Domain.Messages.Reports;
using CommonTestUtilities;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.Users.Update;
public class UpdateUserTest : CashFlowClassFixture
{
    private const string REQUEST_URI = "/api/users";
    private readonly string _token;

    public UpdateUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestsRegisterUserJsonBuilder.Build();
        var response = await DoPut(REQUEST_URI, request, _token);

        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string cultureInfo)
    {
        var request = RequestsRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var response = await DoPut(REQUEST_URI, request, _token, cultureName: cultureInfo);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStreamAsync();

        body.Should().NotBeNull();

        var bodyJson = await JsonDocument.ParseAsync(body);
        var errors = bodyJson.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceReportGenerationMessages.ResourceManager.GetString("NAME_REQUIRED", new System.Globalization.CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(errors => errors.GetString()!.Equals(expectedMessage));
    }
}
