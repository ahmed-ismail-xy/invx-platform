using Invx.SharedKernel.Domain.Primitives.ValueObjects;

namespace Invx.Invoicing.Domain.ValueObjects;
public sealed record RevenueScheduleItem(
        DateTime RecognitionDate,
        Money Amount,
        string Description) : ValueObject;