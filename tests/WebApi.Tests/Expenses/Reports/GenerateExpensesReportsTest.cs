using System.Net.Mime;
using System.Net;
using PdfSharp.Snippets.Font;
using FluentAssertions;
using System.Globalization;

namespace WebApi.Tests.Expenses.Reports;
public class GenerateExpensesReportsTest : CashFlowClassFixture
{
    private const string METHOD = "api/report";

    private readonly string _adminToken;
    private readonly string _teamMemberToken;
    private readonly string _expenseDate;

    public GenerateExpensesReportsTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
    {
        _adminToken = customWebApplicationFactory.UserAdmin.GetToken();
        _teamMemberToken = customWebApplicationFactory.UserTeamMember.GetToken();
        _expenseDate = DateTime.Parse(s: customWebApplicationFactory.ExpenseAdminManager.GetExpense().Date.ToString(), provider: CultureInfo.CurrentCulture)
            .ToString("yyyy/MM/dd"); ;
    }

    [Fact]
    public async Task Success_Pdf()
    {
        var result = await DoGet(requesURI: $"{METHOD}/Expenses/Pdf?month={_expenseDate}", token: _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task Success_Excel()
    {
        var result = await DoGet(requesURI: $"{METHOD}/Expenses/Excel?month={_expenseDate}", token: _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var result = await DoGet(requesURI: $"{METHOD}/Expenses/Excel?month={_expenseDate}", token: _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Pdf()
    {
        var result = await DoGet(requesURI: $"{METHOD}/Expenses/Pdf?month={_expenseDate}", token: _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

}

