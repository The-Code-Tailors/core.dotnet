using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess
{
    public class Milieu
    {
        private const string FUNCTIONLIST = "com.fabioscagliola.Core.DataAccess.Milieu.FunctionList";

        private List<DataAccessFunction> _functionList;

        protected long domainId;
        protected long userId;

        public Milieu(long domainId, long userId)
        {
            this.domainId = domainId;
            this.userId = userId;
        }

        public long DomainId { get { return domainId; } }
        public long UserId { get { return userId; } }

        protected static Milieu systemMilieu;

        static Milieu()
        {
            systemMilieu = new Milieu(0, 1);
        }

        public static Milieu SystemMilieu
        {
            get
            {
                return systemMilieu;
            }
        }

        public List<DataAccessFunction> FunctionList
        {
            get
            {
                UserSqlController controller = (UserSqlController)new User().GetDefaultController();
                _functionList = controller.SelectFunctionList(domainId, userId);
                // TODO: [dotnet] Cache functions in the session (or somewhere else, for that matter, as long as per user)
                return _functionList;
            }
        }

        public static void FlushCache()
        {
            // TODO: [dotnet] Flush the cache
        }

    }
}

