using Invx.SharedKernel.Domain.Primitives.ValueObjects;

namespace Invx.Invoicing.Domain.ValueObjects;
public sealed record Address(
        string Street,
        string City,
        string State,
        string PostalCode,
        string Country) : ValueObject;