namespace Domain;

public abstract class Entity(Guid Id)
{
    public Guid Id { get; } = Id;

    public static bool operator ==(Entity? entityA, Entity? entityB)
    {
        return entityA is not null && entityB is not null && entityA.Equals(entityB);
    }

    public static bool operator !=(Entity? entityA, Entity? entityB)
    {
        return !(entityA == entityB);
    }
    public override bool Equals(object? obj)
    {

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        if (obj is not Entity entity)
        {
            return false;
        }

        return entity.Id == Id;
    }

    public bool Equals(Entity? other)
    {
        if (other == null || GetType() != other.GetType())
        {
            return false;
        }


        return other.Id == Id;

    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
