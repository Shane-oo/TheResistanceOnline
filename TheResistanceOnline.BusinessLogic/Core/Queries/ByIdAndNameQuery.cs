using JetBrains.Annotations;

namespace TheResistanceOnline.BusinessLogic.Core.Queries
{
    public class ByIdAndNameQuery: ByIdQuery
    {
        #region Properties

        [NotNull]
        public string Name { get; set; }

        #endregion
    }
}
