using Invx.SharedKernel.Domain.Primitives.ValueObjects;

namespace Invx.Invoicing.Domain.ValueObjects;
public sealed record BillingParty(
        string CompanyName,
        string TaxNumber,
        Address Address,
        string ContactEmail,
        string ContactPhone) : ValueObject;
