using Invx.Invoicing.Domain.ValueObjects;

namespace Invx.Invoicing.Domain.Aggregates;
public interface IInvoiceRepository
{
    Task<Invoice?> GetByNumberAsync(InvoiceNumber number, CancellationToken cancellationToken = default);
    Task<IEnumerable<Invoice>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync(CancellationToken cancellationToken = default);
    Task<int> GetNextSequenceNumberAsync(CancellationToken cancellationToken = default);
}