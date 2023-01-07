using TheResistanceOnline.Data.Core.Queries;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.BusinessLogic.Users.DbQueries
{
    public interface IUserByNameOrEmailDbQuery: IDbQuery<User>
    {
        IUserByNameOrEmailDbQuery AsNoTracking();

        IUserByNameOrEmailDbQuery Include(string[] include);

        IUserByNameOrEmailDbQuery WithParams(string nameOrEmail);
    }
}
