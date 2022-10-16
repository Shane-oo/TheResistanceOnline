namespace TheResistanceOnline.Data.Entities;

public interface IAuditableEntity
{
    DateTime CreatedOn { get; set; }

    DateTime? ModifiedOn { get; set; }
}
