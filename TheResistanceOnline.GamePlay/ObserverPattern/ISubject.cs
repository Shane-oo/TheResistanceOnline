namespace TheResistanceOnline.GamePlay.ObserverPattern;

public interface ISubject
{
    public void NotifyObservers();

    public void RegisterObserver(IObserver observer);

    public void RemoveObserver(IObserver observer);
}
