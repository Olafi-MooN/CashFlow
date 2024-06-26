using CashFlow.Communication;
using CashFlow.Domain.Messages.Reports;
using ClosedXML.Excel;

namespace CashFlow.Application;

public class GenerateExpenseReportExcelUseCase : IGenerateExpenseReportExcelUseCase
{
    public Task<byte[]> Execute(RequestInFormationReportJson request)
    {
        var xlWorkbook = new XLWorkbook
        {
            Author = "CashFlow"
        };
        xlWorkbook.Style.Font.FontName = "Times New Roman";
        xlWorkbook.Style.Font.FontSize = 12;

        var worksheet = xlWorkbook.Worksheets.Add($"Expenses-{request.Month:Y}");

        InsertHeader(worksheet);

        var stream = new MemoryStream();

        xlWorkbook.SaveAs(stream);

        return Task.FromResult(stream.ToArray());
    }

    private void InsertHeader(IXLWorksheet ixlWorkbook)
    {
        ixlWorkbook.Cell(1, 1).Value = ResourceReportGenerationMessages.TITLE;
        ixlWorkbook.Cell(1, 2).Value = ResourceReportGenerationMessages.DATE;
        ixlWorkbook.Cell(1, 3).Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
        ixlWorkbook.Cell(1, 4).Value = ResourceReportGenerationMessages.AMOUNT;
        ixlWorkbook.Cell(1, 5).Value = ResourceReportGenerationMessages.DESCRIPTION;

        ixlWorkbook.Cells("A1:E1").Style.Font.Bold = true;
        ixlWorkbook.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#f5c2b6");

        ixlWorkbook.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        ixlWorkbook.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        ixlWorkbook.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        ixlWorkbook.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        ixlWorkbook.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

    }
}
