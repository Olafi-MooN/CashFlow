using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Tests;

public class CashFlowClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public CashFlowClassFixture(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    protected async Task<HttpResponseMessage> DoPost(string requesURI, object request, string? token = "", string cultureName = "en-US")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(cultureName);
        return await _httpClient.PostAsJsonAsync(requesURI, request);
    }
    protected async Task<HttpResponseMessage> DoPut(string requesURI, object request, string? token = "", string cultureName = "en-US")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(cultureName);
        return await _httpClient.PutAsJsonAsync(requesURI, request);
    }

    protected async Task<HttpResponseMessage> DoGet(string requesURI, string? token = "", string cultureName = "en-US")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(cultureName);
        return await _httpClient.GetAsync(requesURI);
    }
    protected async Task<HttpResponseMessage> DoDelete(string requesURI, string? token = "", string cultureName = "en-US")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(cultureName);
        return await _httpClient.DeleteAsync(requesURI);
    }

    private void AuthorizeRequest(string? token)
    {
        if (token is not null) _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void ChangeRequestCulture(string cultureName)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(cultureName));
    }
}
