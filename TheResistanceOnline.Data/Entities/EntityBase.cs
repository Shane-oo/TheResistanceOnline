namespace TheResistanceOnline.Data.Entities;

public class EntityBase<TKey>:IEntity<TKey>
{
    public TKey Id { get; set; }
}
