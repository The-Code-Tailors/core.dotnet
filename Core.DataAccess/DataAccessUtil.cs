using com.fabioscagliola.Core.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public static class DataAccessUtil
    {
        public static void AssignFunctionToRole(long roleId, DataAccessFunction function)
        {
            Role role = Role.Select(Milieu.SystemMilieu, roleId);
            role.AssignFunction(Milieu.SystemMilieu, function);
        }

        public static void AssignRoleToUser(string username, long domainId, long roleId)
        {
            User user = User.Select(Milieu.SystemMilieu, username);
            Domain domain = Domain.Select(Milieu.SystemMilieu, domainId);
            Role role = Role.Select(Milieu.SystemMilieu, roleId);
            user.AssignRole(Milieu.SystemMilieu, domain, role);
        }

        public static void RemoveRoleFromUser(string username, long domainId, long roleId)
        {
            User user = User.Select(Milieu.SystemMilieu, username);
            Domain domain = Domain.Select(Milieu.SystemMilieu, domainId);
            Role role = Role.Select(Milieu.SystemMilieu, roleId);
            user.RemoveRole(Milieu.SystemMilieu, domain, role);
        }

        public static Role CreateRole(string name)
        {
            Role role = new Role() { Name = name, };
            role.Update(Milieu.SystemMilieu);
            return role;
        }

        public static User CreateUser(string username, string password, string fName, string lName, long domainId)
        {
            User user = new User() { UName = username, PHash = User.Hash(password), FName = fName, LName = lName, };
            CreateUser(user, domainId);
            return user;
        }

        public static User CreateUser(string username, string password, string fName, string lName, long domainId, SqlTransaction transaction)
        {
            User user = new User() { UName = username, PHash = User.Hash(password), FName = fName, LName = lName, };
            CreateUser(user, domainId, transaction);
            return user;
        }

        public static void CreateUser(User user, long domainId)
        {
            SqlController controller = (SqlController)user.GetDefaultController();
            SqlConnection connection = controller.GetConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                try
                {
                    CreateUser(user, domainId, transaction);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            finally
            {
                transaction.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }

        public static void CreateUser(User user, long domainId, SqlTransaction transaction)
        {
            user.Update(Milieu.SystemMilieu, transaction);
            Domain domain = Domain.Select(Milieu.SystemMilieu, domainId);
            user.AssignDomain(Milieu.SystemMilieu, domain, transaction);
            Role role = Role.Select(Milieu.SystemMilieu, 1);
            user.AssignRole(Milieu.SystemMilieu, domain, role, transaction);
        }

        public static void RemoveFunctionFromRole(long roleId, DataAccessFunction function)
        {
            Role role = Role.Select(Milieu.SystemMilieu, roleId);
            role.RemoveFunction(Milieu.SystemMilieu, function);
        }

    }
}

