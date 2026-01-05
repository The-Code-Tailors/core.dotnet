using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class Role
    {
        protected List<DataAccessFunction> functionList = null;
        protected List<User> userList = null;

        public static Role Select(Milieu milieu, string name)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleSelect);
            RoleSqlController controller = (RoleSqlController)new Role().GetDefaultController();
            return controller.Select(name);
        }

        public override void Update(Milieu milieu, SqlTransaction transaction)
        {
            List<Role> roleList = Role.SelectList(milieu);

            if (!Exists && roleList.Find(x => x.Name == this.Name) != null)
            {
                throw new DataAccessException("A role with the same name already exists!");
            }

            base.Update(milieu, transaction);
        }

        #region Function list management

        public void AssignFunction(Milieu milieu, DataAccessFunction function)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleUpdate);
            RoleFunction roleFunction = RoleFunction.Select(milieu, this.Id, function);
            if (roleFunction.Id == 0)  // Ensure it does not already exist 
            {
                roleFunction = new RoleFunction() { RoleId = this.Id, Function = function, };
                roleFunction.Update(milieu);
                functionList = null;  // Flush cache 
            }
        }

        public void RemoveFunction(Milieu milieu, DataAccessFunction function)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleUpdate);
            RoleFunction roleFunction = RoleFunction.Select(milieu, this.Id, function);
            if (roleFunction.Id != 0)  // Ensure it exists 
            {
                roleFunction.Delete(milieu);
                functionList = null;  // Flush cache 
            }
        }

        public List<DataAccessFunction> SelectFunctionList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleSelect);
            if (functionList == null)
            {
                functionList = new List<DataAccessFunction>();
                List<RoleFunction> roleFunctionList = RoleFunction.SelectList(milieu, this.Id);
                foreach (RoleFunction roleFunction in roleFunctionList)
                {
                    DataAccessFunction function = roleFunction.Function;
                    functionList.Add(function);
                }
            }
            return functionList;
        }

        #endregion

        #region User list management

        public void AssignUser(Milieu milieu, Domain domain, User user)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleUpdate);
            UserRole userRole = UserRole.Select(milieu, domain.Id, user.Id, this.Id);
            if (userRole.Id == 0)  // Ensure it does not already exist 
            {
                userRole = new UserRole() { DomainId = domain.Id, UserId = user.Id, RoleId = this.Id, };
                userRole.Update(milieu);
                userList = null;  // Flush cache 
            }
        }

        public void RemoveUser(Milieu milieu, Domain domain, User user)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleUpdate);
            UserRole userRole = UserRole.Select(milieu, domain.Id, user.Id, this.Id);
            if (userRole.Id != 0)  // Ensure it exists 
            {
                userRole.Delete(milieu);
                userList = null;  // Flush cache 
            }
        }

        public List<User> SelectUserList(Milieu milieu, long domainId)  // TODO: Change the type of the last parameter to Domain 
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleSelect);
            if (userList == null)
            {
                userList = new List<User>();
                List<UserRole> userRoleList = UserRole.SelectListByRoleId(milieu, domainId, this.Id);
                foreach (UserRole userRole in userRoleList)
                {
                    User user = User.Select(milieu, userRole.UserId);
                    userList.Add(user);
                }
            }
            return userList;
        }

        #endregion

    }
}

