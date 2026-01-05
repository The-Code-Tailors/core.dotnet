using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class DomainSqlController : DataAccessSqlController<Domain>
    {
        public DomainSqlController(ControllerConfiguration configuration, Domain entity)
            : base(configuration, entity, "DomainDelete", "DomainInsert", "DomainSelect", "DomainSelectList", "DomainSelectVersionHistory", "DomainSelectVersionHistoryItem", "DomainUpdate")
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

        public Domain Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "DomainSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

    }
}

