using Invx.SharedKernel.Domain.Primitives.ValueObjects;

namespace Invx.Invoicing.Domain.ValueObjects;
public sealed record InvoiceRecipient(
        string CompanyName,
        string ContactPerson,
        string Email,
        Address BillingAddress,
        Address ShippingAddress,
        string TaxNumber) : ValueObject;