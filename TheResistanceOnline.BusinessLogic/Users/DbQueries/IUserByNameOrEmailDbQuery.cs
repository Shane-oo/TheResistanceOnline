using TheResistanceOnline.BusinessLogic.Users.Models;
using TheResistanceOnline.Data.Core;

namespace TheResistanceOnline.BusinessLogic.Users.DbQueries
{
    public interface IUserByNameOrEmailDbQuery: IDbQuery<UserDetailsModel>
    {
        IUserByNameOrEmailDbQuery WithParams(string nameOrEmail);
    }
}
