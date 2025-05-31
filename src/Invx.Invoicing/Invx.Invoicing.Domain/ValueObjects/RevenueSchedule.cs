using Invx.Invoicing.Domain.Enums;
using Invx.SharedKernel.Domain.Primitives.ValueObjects;

namespace Invx.Invoicing.Domain.ValueObjects;
public sealed record RevenueSchedule(
        RevenueRecognitionMethod Method,
        List<RevenueScheduleItem> ScheduleItems) : ValueObject;