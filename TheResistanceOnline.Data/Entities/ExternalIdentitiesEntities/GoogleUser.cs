using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Entities;

public class GoogleUser
{
    #region Properties

    public int Id { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public string Subject { get; set; }

    #endregion
}
