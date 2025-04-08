namespace TestTaskToItKontact.Domain.Models;

public sealed class Loan
{
    public long LoanId { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? DateInactive { get; set; }

    public double InterestRate { get; set; }

    public double PipMt { get; set; }

    public int OriginalAmt { get; set; }

    public int PrincipalBal { get; set; }

    public LoanPropertyType PropertyType { get; set; }
}
