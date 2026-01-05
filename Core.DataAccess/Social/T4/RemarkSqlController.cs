using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class RemarkSqlController : DataAccessSqlController<Remark>
    {
        public RemarkSqlController(ControllerConfiguration configuration, Remark entity)
            : base(configuration, entity, "RemarkDelete", "RemarkInsert", "RemarkSelect", "RemarkSelectList", "RemarkSelectVersionHistory", "RemarkSelectVersionHistoryItem", "RemarkUpdate")
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
            command.Parameters.AddWithValue("@MasterEntity", GetValueOrDBNull(entity.MasterEntity));
            command.Parameters.AddWithValue("@MasterGuid", GetValueOrDBNull(entity.MasterGuid));
            command.Parameters.AddWithValue("@MasterId", GetValueOrDBNull(entity.MasterId));
            command.Parameters.AddWithValue("@Text", GetValueOrDBNull(entity.Text));
        }

        public Remark Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "RemarkSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

        public List<Remark> SelectList(Guid masterGuid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [Remark] WITH (READUNCOMMITTED) WHERE [MasterGuid] = @MasterGuid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@MasterGuid", masterGuid);
            return SelectList(command);
        }

    }
}

