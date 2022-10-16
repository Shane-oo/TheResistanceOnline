namespace TheResistanceOnline.Data.Entities;

public class NamedEntity<TKey>: EntityBase<TKey>, INamed
{
    #region Properties

    public string Name { get; set; }

    #endregion

    #region Public Methods

    public override string ToString()
    {
        return $"{{{Id} - {Name}}}";
    }

    #endregion
}
