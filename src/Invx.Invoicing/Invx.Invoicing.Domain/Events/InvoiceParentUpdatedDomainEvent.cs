using Invx.SharedKernel.Domain.Primitives.Events;

namespace Invx.Invoicing.Domain.Events;
public sealed record InvoiceParentUpdatedDomainEvent(
    Guid TenantId,
    Guid InvoiceId,
    Guid ParentInvoiceId) : DomainEvent;