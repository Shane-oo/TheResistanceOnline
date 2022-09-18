using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.Users.Commands
{
    public class UserResetPasswordCommand: CommandBase
    {
        #region Properties

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        [NotNull]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [NotNull]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [NotNull]
        public string Password { get; set; }

        [NotNull]
        public string Token { get; set; }

        #endregion
    }
}
