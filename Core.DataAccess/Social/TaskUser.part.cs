using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class TaskUser
    {
        public static TaskUser Select(Milieu milieu, long taskId, long userId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskUserSelect);
            TaskUser entity = new TaskUser();
            TaskUserSqlController controller = (TaskUserSqlController)entity.GetDefaultController();
            return controller.Select(taskId, userId);
        }

        public static List<TaskUser> SelectListByTaskId(Milieu milieu, long taskId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskUserSelect);
            TaskUser taskUser = new TaskUser();
            TaskUserSqlController controller = (TaskUserSqlController)taskUser.GetDefaultController();
            return controller.SelectListByTaskId(taskId);
        }

        public static List<TaskUser> SelectListByUserId(Milieu milieu, long userId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskUserSelect);
            TaskUser taskUser = new TaskUser();
            TaskUserSqlController controller = (TaskUserSqlController)taskUser.GetDefaultController();
            return controller.SelectListByUserId(userId);
        }

    }
}

