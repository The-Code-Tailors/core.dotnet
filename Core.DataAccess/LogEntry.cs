using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class LogEntry
    {
        public static List<T> SelectList<T>(Milieu milieu) where T : LogEntry, new()
        {
            EnsureAuthorized(milieu, DataAccessFunction.LogEntrySelect);
            LogEntrySqlController controller = (LogEntrySqlController)new LogEntry().GetDefaultController();
            return controller.SelectList<T>();
        }

        public static List<T> SelectList<T>(Milieu milieu, Guid masterGuid) where T : LogEntry, new()
        {
            EnsureAuthorized(milieu, DataAccessFunction.LogEntrySelect);
            LogEntrySqlController controller = (LogEntrySqlController)new LogEntry().GetDefaultController();
            return controller.SelectList<T>(masterGuid);
        }

    }
}

