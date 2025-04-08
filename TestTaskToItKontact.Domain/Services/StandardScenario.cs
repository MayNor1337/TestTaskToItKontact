using Microsoft.Extensions.Logging;
using TestTaskToItKontact.Domain.DataContracts;
using TestTaskToItKontact.Domain.Helpers;
using TestTaskToItKontact.Domain.Models;
using TestTaskToItKontact.Domain.Results;
using TestTaskToItKontact.Domain.Services.Interfaces;

namespace TestTaskToItKontact.Domain.Services;

internal sealed class StandardScenario : IScenarioStrategy
{
    private readonly IFileHelper _helpService;
    private readonly ICsvService _csvService;
    private readonly IExelService _exelService;
    private readonly IFilesRepository _filesRepository;
    private readonly ILogger<StandardScenario> _logger;

    public StandardScenario(ICsvService csvService, IExelService exelService, IFilesRepository filesRepository, IFileHelper helpService, ILogger<StandardScenario> logger)
    {
        _csvService = csvService;
        _exelService = exelService;
        _filesRepository = filesRepository;
        _helpService = helpService;
        _logger = logger;
    }

    public async Task HandleScenario(string[] arg)
    {
        var filePath = arg[0];
        var getFileResult = _helpService.GetFile(filePath);
        
        if (getFileResult is not GetFileResult.FileFound fileFound)
        {
            _logger.LogInformation("File not found: {0}", filePath);
            return;
        }

        var file = fileFound.File;

        var loans = GetLoans(file);

        var exelFile = GetExelFile(loans);

        await _filesRepository.AddAsync(exelFile);
    }

    private IEnumerable<Loan> GetLoans(IEnumerable<string> file)
    {
        try
        {
            return _csvService.GetLoanFromCsvFile(file);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    private byte[] GetExelFile(IEnumerable<Loan> loans)
    {
        try
        {
            return _exelService.LoanToExcel(loans);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}