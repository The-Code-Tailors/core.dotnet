using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class Task : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.TaskDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.TaskUpdate; } }

        public bool Complete { get; set; }
        public DateTime? DueDate { get; set; }
        public string MasterEntity { get; set; }
        public Guid MasterGuid { get; set; }
        public long MasterId { get; set; }
        public string Text { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            TaskSqlController controller = new TaskSqlController(configuration, this);
            return controller;
        }

        public static Task Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskSelect);
            TaskSqlController controller = (TaskSqlController)new Task().GetDefaultController();
            return controller.Select(id);
        }

        public static Task Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskSelect);
            TaskSqlController controller = (TaskSqlController)new Task().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<Task> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskSelect);
            TaskSqlController controller = (TaskSqlController)new Task().GetDefaultController();
            return controller.SelectList();
        }

        public static List<Task> SelectList(Milieu milieu, Guid masterGuid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskSelect);
            TaskSqlController controller = (TaskSqlController)new Task().GetDefaultController();
            return controller.SelectList(masterGuid);
        }

        public List<Task> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            TaskSqlController controller = (TaskSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public Task SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            TaskSqlController controller = (TaskSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

