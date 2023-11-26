using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Queries;

namespace TheResistanceOnline.Users.Users;

public class GetUserHandler: IQueryHandler<GetUserQuery, UserDetailsModel>
{
    #region Fields

    private readonly IDataContext _context;

    #endregion

    #region Construction

    public GetUserHandler(IDataContext context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<Result<UserDetailsModel>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        if (query == null)
        {
            return Result.Failure<UserDetailsModel>(Error.NullValue);
        }

        var user = await _context.Query<IUserByUserIdDbQuery>()
                                 .WithParams(query.UserId)
                                 .WithNoTracking()
                                 .ExecuteAsync(cancellationToken);

        var notFoundResult = NotFoundError.FailIfNull(user);
        if (notFoundResult.IsFailure)
        {
            return Result.Failure<UserDetailsModel>(notFoundResult.Error);
        }

        var userDetailsModel = new UserDetailsModel
                               {
                                   CreatedOn = user.CreatedOn,
                                   ModifiedOn = user.ModifiedOn,
                                   UserName = user.UserName
                               };

        return userDetailsModel;
    }

    #endregion
}
