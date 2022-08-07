using System.ComponentModel.DataAnnotations;
using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.Users.Commands
{

    public class UserForgotPasswordCommand :CommandBase
    {
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }

        [Required]
        public string? ClientUri { get; set; }
    }
}