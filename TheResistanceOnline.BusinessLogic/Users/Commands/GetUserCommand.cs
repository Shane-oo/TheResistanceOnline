using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.Users.Commands
{
    public class GetUserCommand: CommandBase
    {
        #region Properties

        public string? UserId { get; set; }

        #endregion
    }
}
