namespace TheResistanceOnline.Data.Entities;

public record UserId(Guid Value)
{
    #region Public Methods

    public static UserId New()
    {
        return new UserId(Guid.NewGuid());
    }

    #endregion
}
