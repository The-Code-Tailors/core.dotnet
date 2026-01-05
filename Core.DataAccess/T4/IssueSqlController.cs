using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class IssueSqlController : DataAccessSqlController<Issue>
    {
        public IssueSqlController(ControllerConfiguration configuration, Issue entity)
            : base(configuration, entity, "IssueDelete", "IssueInsert", "IssueSelect", "IssueSelectList", "IssueSelectVersionHistory", "IssueSelectVersionHistoryItem", "IssueUpdate")
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
        }

        public Issue Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "IssueSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

    }
}

