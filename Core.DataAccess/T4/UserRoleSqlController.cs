using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class UserRoleSqlController : DataAccessSqlController<UserRole>
    {
        public UserRoleSqlController(ControllerConfiguration configuration, UserRole entity)
            : base(configuration, entity, "UserRoleDelete", "UserRoleInsert", "UserRoleSelect", "UserRoleSelectList", "UserRoleSelectVersionHistory", "UserRoleSelectVersionHistoryItem", "UserRoleUpdate")
        {
        }

        protected override void DeleteInternal(SqlTransaction transaction, bool permanently)
        {
            if (!permanently)
            {
            }

            base.DeleteInternal(transaction, permanently);
        }

        protected override void DoParameters(SqlCommand command)
        {
            base.DoParameters(command);
            command.Parameters.AddWithValue("@DomainId", GetValueOrDBNull(entity.DomainId));
            command.Parameters.AddWithValue("@RoleId", GetValueOrDBNull(entity.RoleId));
            command.Parameters.AddWithValue("@UserId", GetValueOrDBNull(entity.UserId));
        }

        public UserRole Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "UserRoleSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

        public List<UserRole> SelectList(long domainId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [UserRole] WITH (READUNCOMMITTED) WHERE [DomainId] = @DomainId AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@DomainId", domainId);
            return SelectList(command);
        }

    }
}

