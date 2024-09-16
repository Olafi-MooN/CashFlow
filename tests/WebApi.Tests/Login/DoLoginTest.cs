using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CashFlow.Domain.Messages.Reports;
using CommonTestUtilities;
using FluentAssertions;
using System.Net.Http.Headers;
using WebApi.Tests.InlineData;
using CashFlow.Domain;
using CashFlow.Communication;
using CommonTestUtilities.Requests;
using CashFlow.Exception;

namespace WebApi.Tests.Login;

public class DoLoginTest : CashFlowClassFixture
{
    private const string REQUEST_URI = "/api/login";
    private readonly User _user;
    private readonly string _userPassword;

    public DoLoginTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _user = webApplicationFactory.UserTeamMember.GetUser();
        _userPassword = webApplicationFactory.UserTeamMember.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _user.Email,
            Password = _userPassword
        };

        var response = await DoPost(REQUEST_URI, request);

        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadAsStreamAsync();

        body.Should().NotBeNull();

        var bodyJson = await JsonDocument.ParseAsync(body);
        bodyJson.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();

    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string CultureInfo)
    {

        var request = RequestLoginJsonBuilder.Build();
        var response = await DoPost(REQUEST_URI, request, cultureName: CultureInfo);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var body = await response.Content.ReadAsStreamAsync();

        body.Should().NotBeNull();

        var bodyJson = await JsonDocument.ParseAsync(body);
        var errors = bodyJson.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new System.Globalization.CultureInfo(CultureInfo));
        errors.Should().HaveCount(1).And.Contain(errors => errors.GetString()!.Equals(expectedMessage));
    }
}
