namespace TheResistanceOnline.BusinessLogic.Games.Models;

public interface IResistanceHubSubject
{
    // Attach a game observer to the subject.
    void Attach(IGameObserver observer, string groupName);

    // Detach a game observer from the subject.
    void Detach(string groupName);

    // Notify all game observers about an event.
    void Notify(GameDetailsModel gameDetails, string groupName);
}
