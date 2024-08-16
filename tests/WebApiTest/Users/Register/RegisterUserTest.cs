using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CashFlow.Domain.Messages.Reports;
using CommonTestUtilities;
using FluentAssertions;
using System.Net.Http.Headers;
using WebApiTest.InlineData;

namespace WebApiTest.Users.Register;

public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private const string REQUEST_URI = "/api/users";
    private readonly HttpClient _httpClient;
    public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestsRegisterUserJsonBuilder.Build();
        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

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

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo));
        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStreamAsync();

        body.Should().NotBeNull();

        var bodyJson = await JsonDocument.ParseAsync(body);
        var errors = bodyJson.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceReportGenerationMessages.ResourceManager.GetString("NAME_REQUIRED", new System.Globalization.CultureInfo(CultureInfo));
        errors.Should().HaveCount(1).And.Contain(errors => errors.GetString()!.Equals(expectedMessage));
    }
}
