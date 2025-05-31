using Invx.SharedKernel.Domain.Primitives.ValueObjects;

namespace Invx.Invoicing.Domain.ValueObjects;
public sealed record TaxCalculation(
       TaxConfiguration Configuration,
       Money TaxableAmount,
       Money CalculatedTax,
       DateTime CalculatedAt) : ValueObject;