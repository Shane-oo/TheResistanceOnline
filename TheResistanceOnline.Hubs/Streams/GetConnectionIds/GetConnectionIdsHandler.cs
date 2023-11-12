using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Data;
using TheResistanceOnline.Hubs.Common;

namespace TheResistanceOnline.Hubs.Streams.GetConnectionIds;

public class GetConnectionIdsHandler: IRequestHandler<GetConnectionIdsQuery, List<ConnectionModel>>
{
    #region Fields

    private readonly IDataContext _context;
    private readonly IHubContext<StreamHub, IStreamHub> _streamHubContext;

    #endregion

    #region Construction

    public GetConnectionIdsHandler(IDataContext context, IHubContext<StreamHub, IStreamHub> streamHubContext)
    {
        _context = context;
        _streamHubContext = streamHubContext;
    }

    #endregion

    #region Public Methods

    public async Task<List<ConnectionModel>> Handle(GetConnectionIdsQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);


        return null;
    }

    #endregion
}
