using CsvHelper.Configuration;

namespace TestTaskToItKontact.Domain.Models;

internal sealed class LoanMap : ClassMap<Loan>
{
    public LoanMap()
    {
        Map(m => m.LoanId).Name("LoanID");
        Map(m => m.DueDate);
        Map(m => m.DateInactive);
        Map(m => m.InterestRate);
        Map(m => m.PipMt).Name("PIPmt");
        Map(m => m.OriginalAmt);
        Map(m => m.PrincipalBal);
        Map(m => m.PropertyType).TypeConverter<LoanPropertyTypeConverter>();;
    }
}