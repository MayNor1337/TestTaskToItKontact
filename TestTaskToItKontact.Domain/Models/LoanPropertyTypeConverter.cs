using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace TestTaskToItKontact.Domain.Models;

internal sealed class LoanPropertyTypeConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        return text.Trim().ToLowerInvariant() switch
        {
            "Single Family" or "single family"  => LoanPropertyType.SingleFamily,
            "Condominium" or "condominium" => LoanPropertyType.Condominium
        };
    }
}