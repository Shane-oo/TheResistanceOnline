using TheResistanceOnline.Core.Errors;

namespace TheResistanceOnline.Hubs.Common;

public interface IErrorHub
{
    Task Error(IEnumerable<object> errors);

    Task Error(Error error);
}
