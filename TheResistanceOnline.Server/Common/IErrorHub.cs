using TheResistanceOnline.Core.Errors;

namespace TheResistanceOnline.Server.Common;

public interface IErrorHub
{
    Task Error(Error error);
}
