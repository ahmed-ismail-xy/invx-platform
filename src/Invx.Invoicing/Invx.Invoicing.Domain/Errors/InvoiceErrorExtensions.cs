using Invx.SharedKernel.Domain.Primitives.Errors;

namespace Invx.Invoicing.Domain.Errors;
public static class InvoiceErrorExtensions
{
    public static DomainError WithInvoiceContext(
        this DomainError error,
        Guid invoiceId,
        string? invoiceNumber = null)
    {
        var metadata = error.Metadata ?? [];
        metadata["InvoiceId"] = invoiceId;

        if (!string.IsNullOrEmpty(invoiceNumber))
        {
            metadata["InvoiceNumber"] = invoiceNumber;
        }

        return DomainError.Create(
            error.Code,
            error.Description,
            error.Source,
            metadata);
    }

    public static DomainError WithLineItemContext(
        this DomainError error,
        Guid lineItemId,
        string? lineNumber = null)
    {
        var metadata = error.Metadata ?? [];
        metadata["LineItemId"] = lineItemId;

        if (!string.IsNullOrEmpty(lineNumber))
        {
            metadata["LineNumber"] = lineNumber;
        }

        return DomainError.Create(
            error.Code,
            error.Description,
            error.Source,
            metadata);
    }

    public static DomainError WithAmountContext(
        this DomainError error,
        decimal amount,
        string currency)
    {
        var metadata = error.Metadata ?? [];
        metadata["Amount"] = amount;
        metadata["Currency"] = currency;

        return DomainError.Create(
            error.Code,
            error.Description,
            error.Source,
            metadata);
    }

    public static DomainError WithUserContext(
        this DomainError error,
        Guid userId,
        string? userName = null)
    {
        var metadata = error.Metadata ?? [];
        metadata["UserId"] = userId;

        if (!string.IsNullOrEmpty(userName))
        {
            metadata["UserName"] = userName;
        }

        return DomainError.Create(
            error.Code,
            error.Description,
            error.Source,
            metadata);
    }
}