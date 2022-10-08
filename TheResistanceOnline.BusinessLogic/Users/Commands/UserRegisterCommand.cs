using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.Users.Commands
{
    public class UserRegisterCommand: CommandBase
    {
        #region Properties

        [NotNull]
        public string ClientUri { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        [NotNull]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [NotNull]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [NotNull]
        public string Password { get; set; }

        [Required(ErrorMessage = "UserName is Required")]
        [MaxLength(30,ErrorMessage = "UserName must be less than 30 Characters" )]
        [NotNull]
        public string UserName { get; set; }

        #endregion
    }
}
