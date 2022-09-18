namespace TheResistanceOnline.Data.Exceptions
{
    public class DomainException: Exception
    {
        #region Properties

        private string EntityExternalRef { get; }


        private object EntityId { get; }

        private string EntityName { get; }

        private Type EntityType { get; }

        #endregion

        #region Construction

        public DomainException(Type entityType, string message): base(message)
        {
            EntityType = entityType;
        }

        public DomainException(Type entityType, string entityName, string message): base(message)
        {
            EntityName = entityName;
            EntityType = entityType;
        }

        #endregion
    }
}
