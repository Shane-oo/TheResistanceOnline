namespace TheResistanceOnline.Users.Users.GetUser;

public class UserDetailsModel
{
    #region Properties

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public string UserName { get; set; }

    #endregion
}
