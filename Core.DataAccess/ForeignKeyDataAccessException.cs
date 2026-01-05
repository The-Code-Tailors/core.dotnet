namespace com.fabioscagliola.Core.DataAccess
{
    public class ForeignKeyDataAccessException : DataAccessException
    {
        protected string entityName;

        public string EntityName
        {
            get
            {
                return entityName;
            }
        }

        public ForeignKeyDataAccessException(string entityName)
            : base(string.Format("The deletion failed because at least one related record exists in the \"{0}\" table!", entityName))
        {
            this.entityName = entityName;
        }

    }
}

