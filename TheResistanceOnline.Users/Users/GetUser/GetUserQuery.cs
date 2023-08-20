using JetBrains.Annotations;
using TheResistanceOnline.Core.Requests.Queries;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Users.Users.GetUser;

[UsedImplicitly]
public class GetUserQuery: QueryBase<UserDetailsModel>
{
    #region Construction

    [UsedImplicitly]
    public GetUserQuery()
    {
    }

    public GetUserQuery(Guid userId, Roles role): base(userId, role)
    {
    }

    #endregion
}
