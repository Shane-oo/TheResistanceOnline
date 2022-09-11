using JetBrains.Annotations;

namespace TheResistanceOnline.BusinessLogic.Messages.Models
{
    public class Message
    {
        #region Properties

        [CanBeNull]
        public string Text { get; set; }

        #endregion
    }
}
