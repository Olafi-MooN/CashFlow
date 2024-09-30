using System.Net;
using FluentAssertions;

namespace WebApi.Tests.Users.Delete;
public class DeleteUserTest : CashFlowClassFixture
{
    private const string REQUEST_URI = "/api/users";
    private readonly string _userTeamMemberToken;

    public DeleteUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _userTeamMemberToken = webApplicationFactory.UserTeamMember.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoDelete(REQUEST_URI, _userTeamMemberToken);

        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
