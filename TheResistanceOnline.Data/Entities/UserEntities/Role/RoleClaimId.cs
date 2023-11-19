namespace TheResistanceOnline.Data.Entities;

public record RoleClaimId(int Value)
{
    #region Public Methods

    public static RoleClaimId New()
    {
        return new RoleClaimId(0);
    }

    #endregion
}
