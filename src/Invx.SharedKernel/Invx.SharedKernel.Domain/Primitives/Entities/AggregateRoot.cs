namespace Invx.SharedKernel.Domain.Primitives.Entities;
public abstract class AggregateRoot : Entity
{
    protected AggregateRoot(Guid id) : base(id) { }
    protected AggregateRoot() { }

    public int Version { get; private set; } = 0;

    public void IncrementVersion()
    {
        Version++;
    }
}