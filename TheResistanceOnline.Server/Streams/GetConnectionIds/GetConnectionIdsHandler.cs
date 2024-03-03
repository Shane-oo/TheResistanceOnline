// using MediatR;
// using TheResistanceOnline.Core.Exceptions;
// using TheResistanceOnline.Data.Entities;
//
// namespace TheResistanceOnline.Hubs.Streams;
//
// public class GetConnectionIdsHandler: IRequestHandler<GetConnectionIdsQuery, List<string>>
// {
//     #region Public Methods
//
//     public async Task<List<string>> Handle(GetConnectionIdsQuery query, CancellationToken cancellationToken)
//     {
//         ArgumentNullException.ThrowIfNull(query);
//
//         if (!query.ConnectionIdsToGroupNames.TryGetValue(query.ConnectionId, out var groupName))
//         {
//             throw new NotFoundException("Group Not Found");
//         }
//
//         var connectionIds = query.ConnectionIdsToGroupNames.Where(c => c.Value == groupName
//                                                                        && c.Key != query.ConnectionId)
//                                  .Select(g => g.Key)
//                                  .ToList();
//
//         return connectionIds;
//     }
//
//     #endregion
// }