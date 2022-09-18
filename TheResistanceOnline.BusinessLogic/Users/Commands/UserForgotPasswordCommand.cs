using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.Users.Commands
{

    public class UserForgotPasswordCommand :CommandBase
    {
        [Required(ErrorMessage = "Email is required.")]
        [NotNull]
        public string Email { get; set; }

        [Required]
        [NotNull]
        public string ClientUri { get; set; }
    }
}