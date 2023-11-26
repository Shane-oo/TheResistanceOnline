using JetBrains.Annotations;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Users.Users;

[UsedImplicitly]
public class GetUserQuery: Query<UserDetailsModel>
{
    #region Construction

    public GetUserQuery(UserId userId, Roles userRole): base(userId, userRole)
    {
    }

    public GetUserQuery()
    {
    }

    #endregion
}
