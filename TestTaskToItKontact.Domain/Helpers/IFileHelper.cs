using TestTaskToItKontact.Domain.Results;

namespace TestTaskToItKontact.Domain.Helpers;

public interface IFileHelper
{
    GetFileResult GetFile(string path);
}