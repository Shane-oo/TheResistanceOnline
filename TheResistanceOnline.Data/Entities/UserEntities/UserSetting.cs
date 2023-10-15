namespace TheResistanceOnline.Data.Entities.UserEntities;

public class UserSetting
{
    #region Properties

    public int Id { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    #endregion
}
