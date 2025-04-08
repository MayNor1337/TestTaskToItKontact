using Microsoft.Extensions.Logging;
using TestTaskToItKontact.Domain.Results;

namespace TestTaskToItKontact.Domain.Helpers;

internal sealed class FileHelper : IFileHelper
{
    ILogger<FileHelper> _logger;

    public FileHelper(ILogger<FileHelper> logger)
    {
        _logger = logger;
    }

    public GetFileResult GetFile(string path)
    {
        if (File.Exists(path))
        {
            _logger.LogInformation($"File {path} exists.");

            string[] file;
            try
            {
                file = File.ReadAllLines(path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }

            return new GetFileResult.FileFound(file);
        }
        
        _logger.LogWarning($"File {path} does not exist.");
        return new GetFileResult.FileNotFound();
    }
}