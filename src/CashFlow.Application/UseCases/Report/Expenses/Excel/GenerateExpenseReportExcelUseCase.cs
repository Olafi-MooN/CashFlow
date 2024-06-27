using System.Globalization;
using CashFlow.Communication;
using CashFlow.Domain;
using CashFlow.Domain.Messages.Reports;
using ClosedXML.Excel;

namespace CashFlow.Application;

public class GenerateExpenseReportExcelUseCase : IGenerateExpenseReportExcelUseCase
{
    private readonly IExpensesRepository _repository;

    public GenerateExpenseReportExcelUseCase(IExpensesRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> Execute(RequestInFormationReportJson request)
    {
        var expenses = await _repository.FilterByMonth(request.Month);

        if (expenses.Count == 0) return [];

        using var xlWorkbook = new XLWorkbook
        {
            Author = "CashFlow"
        };
        xlWorkbook.Style.Font.FontName = "Times New Roman";
        xlWorkbook.Style.Font.FontSize = 12;

        var worksheet = xlWorkbook.Worksheets.Add($"Expenses-{request.Month:Y}");


        InsertHeader(worksheet);
        InsertBody(worksheet, expenses);

        worksheet.Column(4).Style.NumberFormat.Format = "R$ #,##0.00";
        worksheet.Columns().AdjustToContents();

        var stream = new MemoryStream();

        xlWorkbook.SaveAs(stream);

        return stream.ToArray();
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
    private void InsertBody(IXLWorksheet ixlWorkbook, List<Expense> expenses)
    {
        var rowIndex = 2;
        foreach (var expense in expenses)
        {
            var row = ixlWorkbook.Row(rowIndex++);
            row.Cell(1).Value = expense.Title;
            row.Cell(2).Value = expense.Date.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"));
            row.Cell(3).Value = expense.PaymentType.ToString();
            row.Cell(4).Value = expense.Amount;
            row.Cell(5).Value = expense.Description;
        }
    }


}
