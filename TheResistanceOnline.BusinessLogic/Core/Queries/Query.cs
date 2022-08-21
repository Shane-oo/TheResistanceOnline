namespace TheResistanceOnline.BusinessLogic.Core.Queries
{
    public class Query: IQuery
    {
        #region Properties

        public CancellationToken CancellationToken { get; set; }

        public string UserId { get; set; }

        #endregion

        #region Construction

        public Query()
        {
            UserId = "";
        }

        public Query(string userId)
        {
            UserId = userId;
        }

        #endregion
    }
}
