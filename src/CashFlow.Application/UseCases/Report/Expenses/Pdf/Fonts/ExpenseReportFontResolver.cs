using System.Reflection;
using DocumentFormat.OpenXml.Presentation;
using PdfSharp.Fonts;

namespace CashFlow.Application;

public class ExpenseReportFontResolver : IFontResolver
{
    public byte[]? GetFont(string faceName)
    {
        var stream = ReadFontFile(faceName) ?? ReadFontFile(FontHelper.DEFAULT_FONT);
        var data = new byte[stream!.Length];

        stream.Read(buffer: data, offset: 0, count: (int)stream!.Length);

        return data;
    }

    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo(familyName);
    }

    private Stream? ReadFontFile(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceStream($"CashFlow.Application.UseCases.Report.Expenses.Pdf.Fonts.{faceName}.ttf");
    }
}
