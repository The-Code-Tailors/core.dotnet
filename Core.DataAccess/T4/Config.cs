using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class Config : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.ConfigDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.ConfigUpdate; } }

        public string Key { get; set; }
        public object Value { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            ConfigSqlController controller = new ConfigSqlController(configuration, this);
            return controller;
        }

        public static Config Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.ConfigSelect);
            ConfigSqlController controller = (ConfigSqlController)new Config().GetDefaultController();
            return controller.Select(id);
        }

        public static Config Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.ConfigSelect);
            ConfigSqlController controller = (ConfigSqlController)new Config().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<Config> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.ConfigSelect);
            ConfigSqlController controller = (ConfigSqlController)new Config().GetDefaultController();
            return controller.SelectList();
        }

        public List<Config> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.ConfigSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            ConfigSqlController controller = (ConfigSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public Config SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.ConfigSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            ConfigSqlController controller = (ConfigSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

