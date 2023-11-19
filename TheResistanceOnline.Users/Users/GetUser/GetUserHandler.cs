using AutoMapper;
using MediatR;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Queries;

namespace TheResistanceOnline.Users.Users;

public class GetUserHandler: IRequestHandler<GetUserQuery, UserDetailsModel>
{
    #region Fields

    private readonly IDataContext _context;

    private readonly IMapper _mapper;

    #endregion

    #region Construction

    public GetUserHandler(IDataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Public Methods

    public async Task<UserDetailsModel> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var user = await _context.Query<IUserByUserIdDbQuery>()
                                 .WithParams(query.UserId)
                                 .WithNoTracking()
                                 .ExecuteAsync(cancellationToken);

        NotFoundException.ThrowIfNull(user);

        return _mapper.Map<UserDetailsModel>(user);
    }

    #endregion
}
