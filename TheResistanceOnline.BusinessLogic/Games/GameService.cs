using TheResistanceOnline.BusinessLogic.Games.Commands;

namespace TheResistanceOnline.BusinessLogic.Games
{
    public interface IGameService
    {
        void CreateNewGameDiscordChatAsync(CreateGameCommand command);

    }

    
    public class GameService: IGameService
    {
        #region Fields


        #endregion

        #region Construction

        public GameService()
        {
          
        }

        #endregion

        #region Public Methods
        
        

        #endregion

        public async void CreateNewGameDiscordChatAsync(CreateGameCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
