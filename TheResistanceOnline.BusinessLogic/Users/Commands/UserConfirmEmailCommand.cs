using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.Users.Commands
{
    public class UserConfirmEmailCommand: CommandBase
    {
        #region Properties

        [NotNull]
        public string Email { get; set; }

        [NotNull]
        public string Token { get; set; }

        #endregion
    }
}
