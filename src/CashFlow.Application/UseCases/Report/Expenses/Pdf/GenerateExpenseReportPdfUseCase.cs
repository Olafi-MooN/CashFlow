using CashFlow.Communication;
using CashFlow.Domain;
using PdfSharp.Fonts;
using MigraDoc.DocumentObjectModel;
using CashFlow.Domain.Messages.Reports;
using System.Drawing;
using Microsoft.VisualBasic;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel.Tables;
using System.Reflection;
using CashFlow.Domain.Services.LoggedUser;


namespace CashFlow.Application;

public class GenerateExpenseReportPdfUseCase : IGenerateExpenseReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private const string HEIGHT_ROW_EXPENSE = "25";
    private readonly IExpensesRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public GenerateExpenseReportPdfUseCase(IExpensesRepository repository, ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;

        GlobalFontSettings.FontResolver = new ExpenseReportFontResolver();
    }
    public async Task<byte[]> Execute(RequestInFormationReportJson request = default!)
    {
        var loggedUserResult = await _loggedUser.Get();
        var expenses = await _repository.FilterByMonth(request.Month, loggedUserResult.Id);

        if (expenses.Count == 0) return [];

        var document = CreateDocument(request.Month, loggedUserResult.Name);
        var page = CreatePage(document);
        CreateHeaderWithProfilePhotoAndName(page, loggedUserResult.Name);

        var totalExpenses = expenses.Sum(x => x.Amount);

        CreateTotalSpentSection(page, request, totalExpenses);

        foreach (var expense in expenses)
        {
            var table = CreateExpenseTable(page);

            var row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE;

            AddExpenseTitle(row.Cells[0], expense.Title);
            AddHeaderForAmount(row.Cells[3]);

            row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE;

            row.Cells[0].AddParagraph(expense.Date.ToString("D"));
            SetStyleBaseForExpenseInformation(row.Cells[0]);

            row.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseForExpenseInformation(row.Cells[1]);

            row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
            SetStyleBaseForExpenseInformation(row.Cells[2]);

            AddAmountForExpense(row.Cells[3], expense.Amount);

            if (!string.IsNullOrEmpty(expense.Description))
            {

                var descriptionRow = table.AddRow();
                descriptionRow.Height = HEIGHT_ROW_EXPENSE;
                descriptionRow.Cells[0].AddParagraph(expense.Description);
                descriptionRow.Cells[0].Format.Font = new Font { Name = FontHelper.WORK_SANS_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
                descriptionRow.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
                descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                descriptionRow.Cells[0].MergeRight = 2;
                descriptionRow.Cells[0].Format.LeftIndent = 20;

                row.Cells[3].MergeDown = 1;
            }

            row = table.AddRow();
            row.Height = 30;
            row.Borders.Visible = false;
        }

        return RenderDocument(document);
    }

    private Document CreateDocument(DateOnly moth, string author)
    {
        var document = new Document();
        document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {moth:MMMM/yyyy}"; document.Info.Author = author;

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.RALEWAY_REGULAR;

        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;

        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;

        return section;
    }

    private byte[] RenderDocument(Document document)
    {
        var render = new PdfDocumentRenderer
        {
            Document = document
        };

        render.RenderDocument();

        using var file = new MemoryStream();
        render.PdfDocument.Save(file);

        return file.ToArray();
    }

    private void CreateHeaderWithProfilePhotoAndName(Section page, string author)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);

        var row = table.AddRow();
        var cellImage = row.Cells[0].AddImage(Path.Combine(directoryName!, "UseCases", "Report", "Expenses", "Pdf", "images", "profile.png"));
        cellImage.Width = 50;
        cellImage.Height = 50;

        row.Cells[1].AddParagraph($"Hey, {author}");
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
        row.Cells[1].Format.Font = new Font(FontHelper.RALEWAY_BLACK, size: 16);
    }

    private Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();

        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }

    private void CreateTotalSpentSection(Section page, RequestInFormationReportJson request, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceAfter = "40";
        paragraph.Format.SpaceBefore = "40";

        var title = $"{string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, request.Month.ToString("Y"))}";
        paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });
        paragraph.AddLineBreak();


        paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {totalExpenses:f2}", new Font { Name = FontHelper.WORK_SANS_BLACK, Size = 50 });
    }

    private void AddExpenseTitle(Cell cell, string title)
    {
        cell.AddParagraph(title);
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }

    private void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleBaseForExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.WORK_SANS_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.LeftIndent = 20;
    }

    private void AddAmountForExpense(Cell cell, decimal amount)
    {
        cell.AddParagraph($"{CURRENCY_SYMBOL} -{amount:f2}");
        cell.Format.Font = new Font { Name = FontHelper.WORK_SANS_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }
}
