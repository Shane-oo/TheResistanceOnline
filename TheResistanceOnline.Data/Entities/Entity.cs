namespace TheResistanceOnline.Data.Entities;

public interface IEntity
{
}

public abstract class Entity<TEntityId>: IEntity
{
    #region Properties

    public TEntityId Id { get; init; }

    #endregion

    #region Construction

    protected Entity(TEntityId id)
    {
        Id = id;
    }

    protected Entity()
    {
    }

    #endregion

    #region Public Methods

    public override string ToString()
    {
        return $"{Id}";
    }

    #endregion
}
