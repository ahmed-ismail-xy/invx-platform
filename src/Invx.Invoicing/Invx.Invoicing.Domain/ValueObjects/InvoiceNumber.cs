using Invx.SharedKernel.Domain.Primitives.ValueObjects;

namespace Invx.Invoicing.Domain.ValueObjects;
public sealed record InvoiceNumber : ValueObject
{
    public string Value { get; private set; }

    private InvoiceNumber(string value)
    {
        Value = value;
    }

    public static InvoiceNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Invoice number cannot be empty", nameof(value));

        if (value.Length > 20)
            throw new ArgumentException("Invoice number cannot exceed 20 characters", nameof(value));

        return new InvoiceNumber(value);
    }

    public static InvoiceNumber Generate(int sequenceNumber, DateTime date)
    {
        var prefix = date.ToString("yyyyMM");
        var sequence = sequenceNumber.ToString("D4");
        return new InvoiceNumber($"INV-{prefix}-{sequence}");
    }

    public override string ToString() => Value;

    public static implicit operator string(InvoiceNumber invoiceNumber) => invoiceNumber.Value;
}