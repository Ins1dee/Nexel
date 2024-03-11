namespace Nexel.Domain.Abstractions;

public abstract class AggragateRoot<TEntityId> : Entity<TEntityId>
{
    protected AggragateRoot(TEntityId id) : base(id)
    {
    }

    protected AggragateRoot()
    {
        // For EF Core
    }
}