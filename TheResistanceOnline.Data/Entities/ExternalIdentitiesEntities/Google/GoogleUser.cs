namespace TheResistanceOnline.Data.Entities;

public class GoogleUser: Entity<GoogleId>
{
    #region Properties

    public User User { get; set; }

    public UserId UserId { get; set; }

    #endregion

    #region Construction

    private GoogleUser(GoogleId googleId): base(googleId)
    {
    }

    public GoogleUser()
    {
    }

    #endregion

    #region Public Methods

    public static GoogleUser Create(GoogleId googleId)
    {
        var googleUser = new GoogleUser(googleId);
        User.Create(googleUser);

        return googleUser;
    }

    #endregion
}
