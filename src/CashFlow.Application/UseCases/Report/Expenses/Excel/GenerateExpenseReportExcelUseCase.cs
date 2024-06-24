using CashFlow.Communication;
using ClosedXML.Excel;

namespace CashFlow.Application;

public class GenerateExpenseReportExcelUseCase : IGenerateExpenseReportExcelUseCase
{
    public Task<byte[]> Execute(RequestInFormationReportJson request)
    {
        byte[] file = new byte[1];

        var xlWorkbook = new XLWorkbook
        {
            Author = "CashFlow"
        };
        xlWorkbook.Style.Font.FontName = "Times New Roman";
        xlWorkbook.Style.Font.FontSize = 12;

        var worksheet = xlWorkbook.Worksheets.Add($"Expenses-{request.Month:Y}");

        InsertHeader(worksheet);

        return Task.FromResult(file);
    }

    private void InsertHeader(IXLWorksheet ixlWorkbook)
    {
        ixlWorkbook.Cell(1, 1).Value = "Date";
        ixlWorkbook.Cell(1, 2).Value = "Category";
        ixlWorkbook.Cell(1, 3).Value = "Description";
        ixlWorkbook.Cell(1, 4).Value = "Amount";
    }
}
