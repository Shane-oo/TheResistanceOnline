using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.UserSettings.Commands
{
    public class UserSettingsUpdateCommand: CommandBase
    {
        #region Properties

        public bool UpdateUserWantsToUseDiscord { get; set; }
        public bool UserWantsToUseDiscord { get; set; }

        
        #endregion
    }
}
