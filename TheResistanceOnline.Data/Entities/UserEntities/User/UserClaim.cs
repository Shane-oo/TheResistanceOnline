namespace TheResistanceOnline.Data.Entities;

public sealed class UserClaim: Entity<UserClaimId>
{
    #region Properties

    public string ClaimType { get; set; }


    public string ClaimValue { get; set; }

    public UserId UserId { get; set; }

    public User User { get; set; }

    #endregion
}
