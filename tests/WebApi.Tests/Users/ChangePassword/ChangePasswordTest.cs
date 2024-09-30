using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Domain.Messages.Reports;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.Users.ChangePassword;
public class ChangePasswordTest : CashFlowClassFixture
{
    private const string REQUEST_URI = "/api/users/change-password";
    private readonly string _token;
    private readonly string _password;

    public ChangePasswordTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
        _password = webApplicationFactory.UserTeamMember.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonFakerBuilder.Build();
        request.OldPassword = _password;

        var response = await DoPut(REQUEST_URI, request, _token);

        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_OldPassword_IsInvalid(string cultureInfo)
    {
        var request = RequestChangePasswordJsonFakerBuilder.Build();

        var response = await DoPut(REQUEST_URI, request, _token, cultureInfo);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStreamAsync();

        body.Should().NotBeNull();

        var bodyJson = await JsonDocument.ParseAsync(body);
        var errors = bodyJson.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("PASSWORD_DIFFERENT_CURRENT_PASSWORD", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(errors => errors.GetString()!.Equals(expectedMessage));
    }
}
