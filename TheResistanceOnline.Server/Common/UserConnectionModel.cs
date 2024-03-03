namespace TheResistanceOnline.Server.Common;

public interface IConnectionModel
{
    public string ConnectionId { get; set; }
}

public class ConnectionModel: IConnectionModel
{
    #region Properties

    public string ConnectionId { get; set; }

    public bool IsReady { get; set; }

    public string UserName { get; set; }

    #endregion
}
