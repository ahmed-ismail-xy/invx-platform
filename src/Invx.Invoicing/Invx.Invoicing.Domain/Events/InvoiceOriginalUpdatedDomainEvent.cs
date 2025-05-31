using Invx.SharedKernel.Domain.Primitives.Events;

namespace Invx.Invoicing.Domain.Events;
public sealed record InvoiceOriginalUpdatedDomainEvent(
    Guid TenantId,
    Guid InvoiceId,
    Guid OriginalInvoiceNumber) : DomainEvent;
