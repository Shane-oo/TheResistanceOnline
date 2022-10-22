namespace TheResistanceOnline.BusinessLogic.Games.Models;

public interface IGameSubject
{
    // Attach a bot observer to the subject.
    void Attach(IBotObserver observer);

    // Detach a bot observer from the subject.
    void Detach(IBotObserver observer);

    // Notify all bot observers about an event.
    void Notify(GameDetailsModel gameDetails);
}
