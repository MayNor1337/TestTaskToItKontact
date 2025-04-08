namespace TestTaskToItKontact.Domain.Results;

public record GetFileResult
{
    private GetFileResult() { }

    public record FileNotFound : GetFileResult;
    
    public record FileFound(IEnumerable<string> File) : GetFileResult;
}