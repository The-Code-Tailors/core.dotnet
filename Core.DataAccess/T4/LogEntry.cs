using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class LogEntry : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.LogEntryDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.LogEntryUpdate; } }

        public string MasterEntity { get; set; }
        public Guid MasterGuid { get; set; }
        public long MasterId { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            LogEntrySqlController controller = new LogEntrySqlController(configuration, this);
            return controller;
        }

        public static LogEntry Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.LogEntrySelect);
            LogEntrySqlController controller = (LogEntrySqlController)new LogEntry().GetDefaultController();
            return controller.Select(id);
        }

        public static LogEntry Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.LogEntrySelect);
            LogEntrySqlController controller = (LogEntrySqlController)new LogEntry().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<LogEntry> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.LogEntrySelect);
            LogEntrySqlController controller = (LogEntrySqlController)new LogEntry().GetDefaultController();
            return controller.SelectList();
        }

        public static List<LogEntry> SelectList(Milieu milieu, Guid masterGuid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.LogEntrySelect);
            LogEntrySqlController controller = (LogEntrySqlController)new LogEntry().GetDefaultController();
            return controller.SelectList(masterGuid);
        }

        public List<LogEntry> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.LogEntrySelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            LogEntrySqlController controller = (LogEntrySqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public LogEntry SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.LogEntrySelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            LogEntrySqlController controller = (LogEntrySqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

