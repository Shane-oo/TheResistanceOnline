using JetBrains.Annotations;
using TheResistanceOnline.Core.Requests.Queries;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Users.Users;

[UsedImplicitly]
public class GetUserQuery: QueryBase<UserDetailsModel>
{
    #region Construction

    [UsedImplicitly]
    public GetUserQuery()
    {
    }

    public GetUserQuery(UserId userId, Roles role): base(userId, role)
    {
    }

    #endregion
}
