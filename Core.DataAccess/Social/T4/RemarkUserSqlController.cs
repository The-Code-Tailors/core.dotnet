using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class RemarkUserSqlController : DataAccessSqlController<RemarkUser>
    {
        public RemarkUserSqlController(ControllerConfiguration configuration, RemarkUser entity)
            : base(configuration, entity, "RemarkUserDelete", "RemarkUserInsert", "RemarkUserSelect", "RemarkUserSelectList", "RemarkUserSelectVersionHistory", "RemarkUserSelectVersionHistoryItem", "RemarkUserUpdate")
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
            command.Parameters.AddWithValue("@RemarkId", GetValueOrDBNull(entity.RemarkId));
            command.Parameters.AddWithValue("@UserId", GetValueOrDBNull(entity.UserId));
        }

        public RemarkUser Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "RemarkUserSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

    }
}

