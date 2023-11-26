namespace TheResistanceOnline.Data.Entities;

public sealed class User: Entity<UserId>, IAuditableEntity
{
    #region Properties

    public int AccessFailedCount { get; set; }

    public Guid ConcurrencyStamp { get; set; } = Guid.NewGuid();

    public DateTimeOffset CreatedOn { get; set; }

    public GoogleUser GoogleUser { get; set; }

    public bool LockoutEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public DateTimeOffset LoginOn { get; set; }

    public MicrosoftUser MicrosoftUser { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public string NormalizedUserName { get; set; }


    public List<PlayerStatistic> PlayerStatistics { get; set; }

    public string SecurityStamp { get; set; }


    public ICollection<UserClaim> UserClaims { get; set; }

    public string UserName { get; set; }

    public UserRole UserRole { get; set; }

    public RedditUser RedditUser { get; set; }

    #endregion

    #region Construction

    public User()
    {
    }

    #endregion

    #region Construction

    private User(UserId id, string userName): base(id)
    {
        UserName = userName;
        NormalizedUserName = userName.Normalize();
    }

    #endregion

    #region Private Methods

    private static string NewUserName()
    {
        return "User" + Ulid.NewUlid();
    }

    #endregion

    #region Public Methods

    public void AddRole(RoleId roleId)
    {
        UserRole = new UserRole(roleId, Id);
    }

    public static User Create(string userName)
    {
        var user = new User(UserId.New(), userName);

        // user.RaiseDomainEvent(new UserCreatedDomainEvent(user.id));

        return user;
    }

    public static User Create()
    {
        var user = new User(UserId.New(), NewUserName());

        return user;
    }

    #endregion
}
