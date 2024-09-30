using System.Net;
using FluentAssertions;

namespace WebApi.Tests.Users.Delete;
public class DeleteUserTest : CashFlowClassFixture
{
    private const string REQUEST_URI = "/api/users";
    private readonly string _userAdminToken;

    public DeleteUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _userAdminToken = webApplicationFactory.UserAdmin.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoDelete(REQUEST_URI, _userAdminToken);

        response.Should().BeSuccessful();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
