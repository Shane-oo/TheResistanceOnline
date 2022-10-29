using System.Diagnostics.CodeAnalysis;

namespace TheResistanceOnline.BusinessLogic.Users.Models
{
    public class UserLoginResponse
    {
        #region Properties

        [NotNull]
        public string Token { get; set; }

        #endregion
    }
}
