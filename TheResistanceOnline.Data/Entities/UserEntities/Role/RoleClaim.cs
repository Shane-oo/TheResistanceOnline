namespace TheResistanceOnline.Data.Entities;

public class RoleClaim: Entity<RoleClaimId>
{
    #region Properties

    public RoleId RoleId { get; set; }

    public Role Role { get; set; }

    public string ClaimType { get; set; }

    public virtual string ClaimValue { get; set; }

    #endregion
}
