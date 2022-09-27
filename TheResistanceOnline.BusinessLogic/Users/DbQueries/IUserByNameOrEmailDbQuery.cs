using TheResistanceOnline.BusinessLogic.Users.Models;
using TheResistanceOnline.Data.Core;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.BusinessLogic.Users.DbQueries
{
    public interface IUserByNameOrEmailDbQuery: IDbQuery<User>
    {
        IUserByNameOrEmailDbQuery WithParams(string nameOrEmail);
    }
}
