namespace Invx.SharedKernel.Domain.Primitives.Events;
public interface IDomainEvent : INotification
{
    Guid Id { get; }

    DateTime OccurredOn { get; }
    
    string EventType { get; }
    
    int Version { get; }
}