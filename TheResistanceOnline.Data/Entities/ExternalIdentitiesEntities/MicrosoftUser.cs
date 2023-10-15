using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Data.Entities.ExternalIdentitiesEntities;

public class MicrosoftUser
{
    #region Properties

    public int Id { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public Guid ObjectId { get; set; }

    #endregion
}
