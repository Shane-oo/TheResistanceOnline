namespace TheResistanceOnline.BusinessLogic.Core.Queries
{
    public class ByIdQuery: ByIdQuery<int>
    {
        #region Construction

        public ByIdQuery()
        {
        }

        public ByIdQuery(string userId, int id): base(userId, id)
        {
        }

        #endregion
    }

    public class ByIdQuery<T>: Query
    {
        #region Properties

        public T Id { get; set; }

        #endregion

        #region Construction

        public ByIdQuery()
        {
        }

        public ByIdQuery(string userId, T id): base(userId)
        {
            Id = id;
        }

        #endregion
    }
}
