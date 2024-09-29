using System.Net;
using System.Text.Json;
using CashFlow.Domain;
using FluentAssertions;

namespace WebApi.Tests.Users.Profile;
public class GetUserProfileTest : CashFlowClassFixture
{
    private const string REQUEST_URI = "/api/users";
    private readonly User _userTeamMember;
    private readonly string _userTeamMemberToken;

    public GetUserProfileTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _userTeamMember = webApplicationFactory.UserTeamMember.GetUser();
        _userTeamMemberToken = webApplicationFactory.UserTeamMember.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoGet(REQUEST_URI, _userTeamMemberToken);

        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStreamAsync();

        body.Should().NotBeNull();
        var bodyJson = await JsonDocument.ParseAsync(body);
        bodyJson.RootElement.GetProperty("name").GetString().Should().Be(_userTeamMember.Name);
        bodyJson.RootElement.GetProperty("email").GetString().Should().Be(_userTeamMember.Email);
    }
}
