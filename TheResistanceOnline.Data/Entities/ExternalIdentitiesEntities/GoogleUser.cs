using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Data.Entities.ExternalIdentitiesEntities;

public class GoogleUser
{
    #region Properties

    public int Id { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public Guid Subject { get; set; }

    #endregion
}
