using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class Task
    {
        protected List<User> userList = null;

        public static List<Task> SelectListByComplete(Milieu milieu, Guid masterGuid, bool complete)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskSelect);
            TaskSqlController controller = (TaskSqlController)new Task().GetDefaultController();
            return controller.SelectListByComplete(masterGuid, complete);
        }

        public static List<Task> SelectList(Milieu milieu, long userId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskSelect);
            List<Task> taskList = new List<Task>();
            List<TaskUser> taskUserList = TaskUser.SelectListByUserId(milieu, userId);
            foreach (TaskUser taskUser in taskUserList)
            {
                Task task = Task.Select(milieu, taskUser.TaskId);

                if (task.Id != 0)  // TODO: [Fabio] Delete TaskUser records when deleting Task records 
                {
                    taskList.Add(task);
                }
            }
            return taskList;
        }

        #region User list management

        public void AssignUser(Milieu milieu, User user)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskUpdate);
            TaskUser taskUser = TaskUser.Select(milieu, this.Id, user.Id);
            if (taskUser.Id == 0)  // Ensure it does not already exist 
            {
                taskUser = new TaskUser() { TaskId = this.Id, UserId = user.Id, };
                taskUser.Update(milieu);
                userList = null;  // Flush cache 

                TaskUserNotification notification = new TaskUserNotification();
                notification.MasterEntity = this.MasterEntity;
                notification.MasterGuid = this.MasterGuid;
                notification.MasterId = this.MasterId;
                notification.EntityInsertUserId = milieu.UserId;
                notification.Text = string.Format("{0} assigned a task to you.", User.Select(Milieu.SystemMilieu, milieu.UserId).FullName);  // TODO: RESX 
                notification.TypeName = typeof(TaskUserNotification).ToString();
                notification.UserId = user.Id;
                notification.Update(Milieu.SystemMilieu);

            }
        }

        public void RemoveUser(Milieu milieu, User user)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskUpdate);
            TaskUser taskUser = TaskUser.Select(milieu, this.Id, user.Id);
            if (taskUser.Id != 0)  // Ensure it exists 
            {
                taskUser.Delete(milieu);
                userList = null;  // Flush cache 
            }
        }

        public List<User> SelectUserList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.TaskSelect);
            if (userList == null)
            {
                userList = new List<User>();
                List<TaskUser> taskUserList = TaskUser.SelectListByTaskId(milieu, this.Id);
                foreach (TaskUser taskUser in taskUserList)
                {
                    User user = User.Select(milieu, taskUser.UserId);
                    userList.Add(user);
                }
            }
            return userList;
        }

        #endregion

    }
}

