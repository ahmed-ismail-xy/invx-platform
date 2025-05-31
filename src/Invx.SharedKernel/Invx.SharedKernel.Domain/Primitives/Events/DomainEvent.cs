namespace Invx.SharedKernel.Domain.Primitives.Events;
public abstract record DomainEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();

    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    
    public string EventType => GetType().Name;
    
    public int Version { get; init; } = 1;
}