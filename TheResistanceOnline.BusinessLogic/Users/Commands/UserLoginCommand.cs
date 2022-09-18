using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.Users.Commands
{
    public class UserLoginCommand: CommandBase
    {
        #region Properties

        [NotNull]
        public string ClientUri { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [NotNull]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [NotNull]
        public string Password { get; set; }

        #endregion
    }
}
