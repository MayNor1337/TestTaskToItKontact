using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TestTaskToItKontact.Domain.Models;
using TestTaskToItKontact.Domain.Services.Interfaces;

namespace TestTaskToItKontact.Domain.Services;

internal sealed class CsvService : ICsvService
{
    public ICollection<Loan> GetLoanFromCsvFile(IEnumerable<string> file)
    {
        var joinedFile = string.Join(Environment.NewLine, file);

        using var reader = new StringReader(joinedFile);

        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
        });

        csv.Context.RegisterClassMap<LoanMap>();

        return csv.GetRecords<Loan>().ToArray();
    }
}