using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class Domain : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.DomainDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.DomainUpdate; } }

        public string Name { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            DomainSqlController controller = new DomainSqlController(configuration, this);
            return controller;
        }

        public static Domain Select1(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.DomainSelect);
            DomainSqlController controller = (DomainSqlController)new Domain().GetDefaultController();
            return controller.Select(id);
        }

        public static Domain Select1(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.DomainSelect);
            DomainSqlController controller = (DomainSqlController)new Domain().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<Domain> SelectList1(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.DomainSelect);
            DomainSqlController controller = (DomainSqlController)new Domain().GetDefaultController();
            return controller.SelectList();
        }

        public List<Domain> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.DomainSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            DomainSqlController controller = (DomainSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public Domain SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.DomainSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            DomainSqlController controller = (DomainSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

