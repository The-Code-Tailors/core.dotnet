using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class TaskUser : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.TaskUserDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.TaskUserUpdate; } }

        public long TaskId { get; set; }
        public long UserId { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            TaskUserSqlController controller = new TaskUserSqlController(configuration, this);
            return controller;
        }

        public static TaskUser Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskUserSelect);
            TaskUserSqlController controller = (TaskUserSqlController)new TaskUser().GetDefaultController();
            return controller.Select(id);
        }

        public static TaskUser Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskUserSelect);
            TaskUserSqlController controller = (TaskUserSqlController)new TaskUser().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<TaskUser> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskUserSelect);
            TaskUserSqlController controller = (TaskUserSqlController)new TaskUser().GetDefaultController();
            return controller.SelectList();
        }

        public List<TaskUser> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskUserSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            TaskUserSqlController controller = (TaskUserSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public TaskUser SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskUserSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            TaskUserSqlController controller = (TaskUserSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

