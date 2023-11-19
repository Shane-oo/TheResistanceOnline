namespace TheResistanceOnline.Data.Entities;

public class MicrosoftUser: Entity<MicrosoftId>
{
    #region Properties

    public User User { get; set; }

    public UserId UserId { get; set; }

    #endregion

    #region Construction

    public MicrosoftUser()
    {
    }

    #endregion

    #region Construction

    private MicrosoftUser(MicrosoftId objectId): base(objectId)
    {
    }

    #endregion

    #region Public Methods

    public static MicrosoftUser Create(MicrosoftId microsoftId)
    {
        var microsoftUser = new MicrosoftUser(microsoftId);
        User.Create(microsoftUser);
        return microsoftUser;
    }

    #endregion
}
