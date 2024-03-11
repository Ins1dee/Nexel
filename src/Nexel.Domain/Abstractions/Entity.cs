namespace Nexel.Domain.Abstractions;

public abstract class Entity<TEntityId>
{
    protected Entity(TEntityId id)
    {
        Id = id;
    }

    protected Entity()
    {
        // For EF Core
    }

    public TEntityId Id { get; init; }
}