using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Entities;

public class MicrosoftUser
{
    #region Properties

    public int Id { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public Guid ObjectId { get; set; }

    #endregion
}
