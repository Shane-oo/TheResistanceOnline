using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.Users.Commands
{

    public class UserConfirmEmailCommand :CommandBase
    {
        public string? Email { get; set; }
        
        public string? Token { get; set; }

    }
}