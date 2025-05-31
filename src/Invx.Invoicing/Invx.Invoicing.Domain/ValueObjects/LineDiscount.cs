using Invx.Invoicing.Domain.Enums;
using Invx.SharedKernel.Domain.Primitives.ValueObjects;

namespace Invx.Invoicing.Domain.ValueObjects;
public sealed record LineDiscount(
        DiscountType Type,
        decimal Value,
        string Reason,
        Money DiscountAmount) : ValueObject;
