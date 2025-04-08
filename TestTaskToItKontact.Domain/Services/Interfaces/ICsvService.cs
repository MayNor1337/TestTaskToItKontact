using TestTaskToItKontact.Domain.Models;

namespace TestTaskToItKontact.Domain.Services.Interfaces;

public interface ICsvService
{
    ICollection<Loan> GetLoanFromCsvFile(IEnumerable<string> file);
}