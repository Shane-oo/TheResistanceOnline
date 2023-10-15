namespace TheResistanceOnline.Hubs.Common;

public interface IErrorHub
{
    Task Error(string errorMessage);
}
