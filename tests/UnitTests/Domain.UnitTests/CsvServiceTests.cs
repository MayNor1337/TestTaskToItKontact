using TestTaskToItKontact.Domain.Models;
using TestTaskToItKontact.Domain.Services;

namespace Domain.UnitTests;

[TestFixture]
internal sealed class CsvServiceTests
{
    private CsvService _csvService;

    [SetUp]
    public void Setup()
    {
        _csvService = new CsvService();
    }

    [Test]
    public void GetLoanFromCsvFile_Should_ParseValidCsv()
    {
        const int loadId = 1;
        const double interestRate = 3.5;
        const LoanPropertyType property = LoanPropertyType.SingleFamily;
        var loanDate = new DateTime(2023, 1, 1);
        
        var csvLines = new[]
        {
            "LoanID;DueDate;DateInactive;InterestRate;PIPmt;OriginalAmt;PrincipalBal;PropertyType",
            $"{loadId};2023-01-01;{loanDate};3.5;500.00;100000;90000;single family"
        };

        var result = _csvService.GetLoanFromCsvFile(csvLines);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));

        var loan = result.FirstOrDefault();

        Assert.That(loan, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(loan.LoanId, Is.EqualTo(loadId));
            Assert.That(loan.DueDate, Is.EqualTo(loanDate));
            Assert.That(loan.InterestRate, Is.EqualTo(interestRate));
            Assert.That(loan.PropertyType, Is.EqualTo(property));
        });
    }

    [Test]
    public void GetLoanFromCsvFile_ShouldReturnEmpty_BecauseOnlyHeader()
    {
        var csvLines = new[]
        {
            "LoanID;DueDate;DateInactive;InterestRate;PIPmt;OriginalAmt;PrincipalBal;PropertyType"
        };

        var result = _csvService.GetLoanFromCsvFile(csvLines);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetLoanFromCsvFile_ShouldThrow_BecauseInvalidData()
    {
        var csvLines = new[]
        {
            "LoanID;DueDate;DateInactive;InterestRate;PIPmt;OriginalAmt;PrincipalBal;PropertyType",
            "abc;invalid-date;;;??;;;"
        };
        
        Assert.Throws<CsvHelper.TypeConversion.TypeConverterException>(
            () => _csvService.GetLoanFromCsvFile(csvLines));
    }
}