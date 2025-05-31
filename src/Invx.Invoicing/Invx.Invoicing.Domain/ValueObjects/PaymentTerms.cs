using Invx.Invoicing.Domain.Enums;
using Invx.SharedKernel.Domain.Primitives.ValueObjects;

namespace Invx.Invoicing.Domain.ValueObjects;
public sealed record PaymentTerms(
        PaymentTermsType TermsType,
        int Days,
        decimal? EarlyPaymentDiscountPercentage = null,
        int? EarlyPaymentDiscountDays = null) : ValueObject
{
    public DateTime CalculateDueDate(DateTime issueDate)
    {
        return TermsType switch
        {
            PaymentTermsType.DueOnReceipt => issueDate,
            PaymentTermsType.Net => issueDate.AddDays(Days),
            PaymentTermsType.EndOfMonth => new DateTime(issueDate.Year, issueDate.Month, DateTime.DaysInMonth(issueDate.Year, issueDate.Month)),
            _ => throw new ArgumentException($"Unknown payment terms type: {TermsType}")
        };
    }

    public bool HasEarlyPaymentDiscount => EarlyPaymentDiscountPercentage.HasValue && EarlyPaymentDiscountDays.HasValue;

    public DateTime? GetEarlyPaymentDiscountDate(DateTime issueDate)
    {
        return HasEarlyPaymentDiscount ? issueDate.AddDays(EarlyPaymentDiscountDays.Value) : null;
    }
}