using JetBrains.Annotations;

namespace TheResistanceOnline.BusinessLogic.Users.Models
{
    public class RegistrationResponse
    {
        #region Properties

        [CanBeNull]
        public IEnumerable<string> Errors { get; set; }

        #endregion
    }
}
