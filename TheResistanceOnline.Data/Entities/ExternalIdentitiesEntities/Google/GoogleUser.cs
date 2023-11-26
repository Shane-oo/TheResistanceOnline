namespace TheResistanceOnline.Data.Entities;

public class GoogleUser: Entity<GoogleId>
{
    #region Properties

    public User User { get; set; }

    public UserId UserId { get; set; }

    #endregion

    #region Construction

    public GoogleUser()
    {
    }

    #endregion

    #region Construction

    private GoogleUser(GoogleId googleId): base(googleId)
    {
    }

    #endregion

    #region Public Methods

    public static GoogleUser Create(GoogleId googleId)
    {
        var googleUser = new GoogleUser(googleId);
        var user = User.Create();
        googleUser.User = user;
        user.GoogleUser = googleUser;

        return googleUser;
    }

    #endregion
}
