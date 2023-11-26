using TheResistanceOnline.Core.Errors;

namespace TheResistanceOnline.Hubs.Common;

public interface IErrorHub
{
    Task Error(Error error);
}
