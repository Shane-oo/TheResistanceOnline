using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.Games.Commands;

namespace TheResistanceOnline.BusinessLogic.Games
{
    public interface IGameService
    {
        Task CreateGameAsync([NotNull] CreateGameCommand command);
    }

    public class GameService  :IGameService
    {
        public async Task CreateGameAsync(CreateGameCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
