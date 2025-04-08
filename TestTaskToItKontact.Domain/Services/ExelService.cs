using ClosedXML.Excel;
using TestTaskToItKontact.Domain.Models;
using TestTaskToItKontact.Domain.Services.Interfaces;

namespace TestTaskToItKontact.Domain.Services;

internal sealed class ExelService : IExelService
{
    public byte[] LoanToExcel(IEnumerable<Loan> loans)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Loans");

        AddHeaders(worksheet);

        AddData(loans, worksheet);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray(); 
    }

    private static void AddData(IEnumerable<Loan> loans, IXLWorksheet worksheet)
    {
        var row = 2;

        foreach (var loan in loans)
        {
            worksheet.Cell(row, 1).Value = loan.LoanId;
            worksheet.Cell(row, 2).Value = loan.DueDate;
            worksheet.Cell(row, 3).Value = loan.DateInactive;
            worksheet.Cell(row, 4).Value = loan.InterestRate;
            worksheet.Cell(row, 5).Value = loan.PipMt;
            worksheet.Cell(row, 6).Value = loan.OriginalAmt;
            worksheet.Cell(row, 7).Value = loan.PrincipalBal;
            worksheet.Cell(row, 8).Value = loan.PropertyType.ToString();
            row++;
        }

        worksheet.Columns().AdjustToContents();
    }

    private static void AddHeaders(IXLWorksheet worksheet)
    {
        worksheet.Cell(1, 1).Value = "LoanId";
        worksheet.Cell(1, 2).Value = "DueDate";
        worksheet.Cell(1, 3).Value = "DateInactive";
        worksheet.Cell(1, 4).Value = "InterestRate";
        worksheet.Cell(1, 5).Value = "PIPmt";
        worksheet.Cell(1, 6).Value = "OriginalAmt";
        worksheet.Cell(1, 7).Value = "PrincipalBal";
        worksheet.Cell(1, 8).Value = "PropertyType";
    }
}