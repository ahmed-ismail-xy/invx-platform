using Invx.SharedKernel.Domain.Primitives.Events;

namespace Invx.Invoicing.Domain.Events;
public sealed record InvoiceCreatedDomainEvent(
    Guid TenantId,
    Guid InvoiceId,
    string InvoiceNumber) : DomainEvent;