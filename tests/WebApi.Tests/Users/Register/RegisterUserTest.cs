using System.Net;
using System.Text.Json;
using CashFlow.Domain.Messages.Reports;
using CommonTestUtilities;
using FluentAssertions;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.Users.Register;

public class RegisterUserTest : CashFlowClassFixture
{
    private const string REQUEST_URI = "/api/users";
    public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestsRegisterUserJsonBuilder.Build();
        var response = await DoPost(REQUEST_URI, request);

        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadAsStreamAsync();

        body.Should().NotBeNull();
        var bodyJson = await JsonDocument.ParseAsync(body);
        bodyJson.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        bodyJson.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string CultureInfo)
    {
        var request = RequestsRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var response = await DoPost(REQUEST_URI, request, cultureName: CultureInfo);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStreamAsync();

        body.Should().NotBeNull();

        var bodyJson = await JsonDocument.ParseAsync(body);
        var errors = bodyJson.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceReportGenerationMessages.ResourceManager.GetString("NAME_REQUIRED", new System.Globalization.CultureInfo(CultureInfo));
        errors.Should().HaveCount(1).And.Contain(errors => errors.GetString()!.Equals(expectedMessage));
    }
}
