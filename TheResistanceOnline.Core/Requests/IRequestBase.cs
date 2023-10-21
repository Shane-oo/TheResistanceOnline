using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Core.Requests;

public interface IRequestBase
{
    public Guid UserId { get; set; }

    public string ConnectionId { get; set; }
}
