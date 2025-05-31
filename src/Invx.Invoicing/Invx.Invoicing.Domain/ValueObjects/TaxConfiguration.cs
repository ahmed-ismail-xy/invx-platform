using Invx.SharedKernel.Domain.Primitives.ValueObjects;

namespace Invx.Invoicing.Domain.ValueObjects;
public sealed record TaxConfiguration(
        string TaxJurisdiction,
        string TaxType,
        decimal TaxRate,
        string TaxRateName,
        bool IsCompoundTax = false,
        bool IsExempt = false,
        string ExemptionReason = null) : ValueObject;
