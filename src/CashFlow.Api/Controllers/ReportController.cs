using System.Net.Mime;
using CashFlow.Application;
using CashFlow.Communication;
using CashFlow.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ADMIN)]
public class ReportController : ControllerBase
{

    [HttpGet("Expenses/Excel")]
    [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel(
        [FromQuery] RequestInFormationReportJson queryParams,
        [FromServices] IGenerateExpenseReportExcelUseCase useCase
        )
    {
        byte[] file = await useCase.Execute(queryParams);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Octet, "report.xlsx");

        return NoContent();
    }

    [HttpGet("Expenses/Pdf")]
    [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdf(
        [FromQuery] RequestInFormationReportJson queryParams,
        [FromServices] IGenerateExpenseReportPdfUseCase useCase
        )
    {
        byte[] file = await useCase.Execute(queryParams);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Pdf, "report.pdf");

        return NoContent();
    }
}
