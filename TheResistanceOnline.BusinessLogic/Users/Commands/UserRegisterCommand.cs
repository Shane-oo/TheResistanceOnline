using System.ComponentModel.DataAnnotations;
using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.Users.Commands
{
    public class UserRegisterCommand: CommandBase
    {
        #region Properties

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "UserName is Required")]
        public string? UserName { get; set; }
        
        public string? ClientUri { get; set; }


        #endregion
    }
}
