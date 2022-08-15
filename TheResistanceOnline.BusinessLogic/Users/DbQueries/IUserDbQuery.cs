using TheResistanceOnline.BusinessLogic.Users.Models;
using TheResistanceOnline.Data.Core;

namespace TheResistanceOnline.BusinessLogic.Users.DbQueries
{
    public interface IUserDbQuery: IDbQuery<UserDetails>
    {
        IUserDbQuery WithParams(string userId);
    }
}
