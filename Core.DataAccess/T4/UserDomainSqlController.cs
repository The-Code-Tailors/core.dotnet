using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class UserDomainSqlController : DataAccessSqlController<UserDomain>
    {
        public UserDomainSqlController(ControllerConfiguration configuration, UserDomain entity)
            : base(configuration, entity, "UserDomainDelete", "UserDomainInsert", "UserDomainSelect", "UserDomainSelectList", "UserDomainSelectVersionHistory", "UserDomainSelectVersionHistoryItem", "UserDomainUpdate")
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
            command.Parameters.AddWithValue("@UserId", GetValueOrDBNull(entity.UserId));
        }

        public UserDomain Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "UserDomainSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

        public List<UserDomain> SelectList(long domainId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [UserDomain] WITH (READUNCOMMITTED) WHERE [DomainId] = @DomainId AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@DomainId", domainId);
            return SelectList(command);
        }

    }
}

