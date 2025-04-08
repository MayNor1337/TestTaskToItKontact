using ClosedXML.Excel;
using TestTaskToItKontact.Domain.Models;
using TestTaskToItKontact.Domain.Services;

namespace Domain.UnitTests;

[TestFixture]
internal sealed class ExelServiceTests
{
    private ExelService _exelService;

    [SetUp]
    public void Setup()
    {
        _exelService = new ExelService();
    }

    [Test]
    public void LoanToExcel_Should_Create_NonEmpty_File()
    {
        var loans = new List<Loan>
        {
            new()
            {
                LoanId = 1,
                DueDate = new DateTime(2023, 1, 1),
                DateInactive = null,
                InterestRate = 3.5,
                PipMt = 400.0,
                OriginalAmt = 100000,
                PrincipalBal = 95000,
                PropertyType = LoanPropertyType.SingleFamily
            }
        };

        var result = _exelService.LoanToExcel(loans);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.GreaterThan(0));

        using var stream = new MemoryStream(result);
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheets.First();

        Assert.That(worksheet.Cell(1, 1).Value.ToString(), Is.EqualTo("LoanId"));
        Assert.That(worksheet.Cell(2, 1).Value.ToString(), Is.EqualTo("1"));
        Assert.That(worksheet.Cell(2, 8).Value.ToString(), Is.EqualTo("SingleFamily"));
    }

    [Test]
    public void LoanToExcel_Should_Work_With_Empty_List()
    {
        var emptyLoans = new List<Loan>();

        var result = _exelService.LoanToExcel(emptyLoans);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.GreaterThan(0));

        using var stream = new MemoryStream(result);
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheets.First();

        Assert.That(worksheet.Cell(1, 1).Value.ToString(), Is.EqualTo("LoanId"));
        Assert.That(worksheet.Cell(2, 1).Value.ToString(), Is.EqualTo(""));
    }
}