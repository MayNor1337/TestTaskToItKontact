using Microsoft.Extensions.Logging;
using Moq;
using TestTaskToItKontact.Domain.DataContracts;
using TestTaskToItKontact.Domain.Helpers;
using TestTaskToItKontact.Domain.Models;
using TestTaskToItKontact.Domain.Results;
using TestTaskToItKontact.Domain.Services;
using TestTaskToItKontact.Domain.Services.Interfaces;

namespace Domain.UnitTests;

[TestFixture]
internal sealed class StandardScenarioTests
{
    private Mock<IFileHelper> _fileHelperMock;
    private Mock<ICsvService> _csvServiceMock;
    private Mock<IExelService> _exelServiceMock;
    private Mock<IFilesRepository> _repositoryMock;
    private Mock<ILogger<StandardScenario>> _loggerMock;

    private StandardScenario _scenario;

    [SetUp]
    public void Setup()
    {
        _fileHelperMock = new Mock<IFileHelper>();
        _csvServiceMock = new Mock<ICsvService>();
        _exelServiceMock = new Mock<IExelService>();
        _repositoryMock = new Mock<IFilesRepository>();
        _loggerMock = new Mock<ILogger<StandardScenario>>();
        
        _scenario = new StandardScenario(
            _csvServiceMock.Object,
            _exelServiceMock.Object,
            _repositoryMock.Object,
            _fileHelperMock.Object,
            _loggerMock.Object);
    }

    [Test]
    public async Task HandleScenario_FileNotFound_LogsAndReturns()
    {
        var args = new[] { "nonexistent.csv" };

        _fileHelperMock
            .Setup(h => h.GetFile("nonexistent.csv"))
            .Returns(new GetFileResult.FileNotFound());

        await _scenario.HandleScenario(args);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<byte[]>()), Times.Never);
        _loggerMock.Verify(
            l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("nonexistent.csv")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Test]
    public async Task HandleScenario_ValidFlow_CallsAddAsync()
    {
        var args = new[] { "valid.csv" };
        var fakeFile = new List<string> 
        { 
            "LoanID;DueDate;DateInactive;InterestRate;PIPmt;OriginalAmt;PrincipalBal;PropertyType",
            "1;2023-01-01;;3.5;500;100000;95000;SingleFamily"
            
        };

        _fileHelperMock
            .Setup(h => h.GetFile("valid.csv"))
            .Returns(new GetFileResult.FileFound(fakeFile));

        _csvServiceMock
            .Setup(s => s.GetLoanFromCsvFile(fakeFile))
            .Returns(new List<Loan> { new Loan { LoanId = 1 } });

        _exelServiceMock
            .Setup(s => s.LoanToExcel(It.IsAny<IEnumerable<Loan>>()))
            .Returns([1, 2, 3]);

        await _scenario.HandleScenario(args);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<byte[]>()), Times.Once);
    }

    [Test]
    public void GetLoans_Throws_LogsError()
    {
        var fakeFile = new List<string> { "bad,data" };

        _fileHelperMock
            .Setup(h => h.GetFile(It.IsAny<string>()))
            .Returns(new GetFileResult.FileFound(fakeFile));

        _csvServiceMock
            .Setup(s => s.GetLoanFromCsvFile(fakeFile))
            .Throws(new FormatException("bad format"));

        var args = new[] { "bad.csv" };

        Assert.ThrowsAsync<FormatException>(async () => await _scenario.HandleScenario(args));

        _loggerMock.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("bad format")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}