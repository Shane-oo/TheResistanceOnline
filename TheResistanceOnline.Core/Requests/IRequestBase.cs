using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Core.Requests;

public interface IRequestBase
{
    public string ConnectionId { get; set; }

    public UserId UserId { get; set; }
}
