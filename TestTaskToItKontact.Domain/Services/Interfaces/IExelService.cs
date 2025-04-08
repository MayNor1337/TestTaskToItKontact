using TestTaskToItKontact.Domain.Models;

namespace TestTaskToItKontact.Domain.Services.Interfaces;

public interface IExelService
{
    byte[] LoanToExcel(IEnumerable<Loan> loan);
}