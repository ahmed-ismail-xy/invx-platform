using Invx.SharedKernel.Domain.Primitives.Events;

namespace Invx.Invoicing.Domain.Events;
public sealed record InvoiceLineItemAddedDomainEvent(
    Guid TenantId,
    Guid InvoiceId,
    Guid LineItemId) : DomainEvent;