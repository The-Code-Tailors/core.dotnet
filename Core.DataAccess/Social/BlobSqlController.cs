using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class BlobSqlController : DataAccessSqlController<Blob>
    {
        public BlobSqlController(ControllerConfiguration configuration, Blob entity)
            : base(configuration, entity, "BlobDelete", "BlobInsert", "BlobSelect", "BlobSelectList", "BlobSelectVersionHistory", "BlobUpdate")
        {
        }

        protected override void DoParameters(SqlCommand command)
        {
            base.DoParameters(command);
            command.Parameters.AddWithValue("@Content", entity.Content);
            command.Parameters.AddWithValue("@ContentLength", entity.ContentLength);
            command.Parameters.AddWithValue("@ContentType", entity.ContentType);
            command.Parameters.AddWithValue("@IsCompressed", entity.IsCompressed);
            command.Parameters.AddWithValue("@MasterEntity", entity.MasterEntity);
            command.Parameters.AddWithValue("@MasterGuid", entity.MasterGuid);
            command.Parameters.AddWithValue("@MasterId", entity.MasterId);
            command.Parameters.AddWithValue("@Name", entity.Name);
        }

        public override SqlConnection GetConnection()
        {
            switch (Controller.Type)
            {
                case ControllerType.SqlLocalDB:
                    {
                        return base.GetConnection();
                    }
                case ControllerType.SqlIntegratedSecurity:
                case ControllerType.Sql:
                default:
                    {
                        SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(base.GetConnection().ConnectionString);
                        sqlConnectionStringBuilder.InitialCatalog += ".Blob";
                        return new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
                    }
            }
        }

        public Blob Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "BlobSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

        public byte[] SelectContent()
        {
            SqlCommand command = new SqlCommand() { CommandText = "BlobSelectContent", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Id", entity.Id);

            byte[] content = null;

            SqlConnection connection = GetConnection();
            try
            {
                command.Connection = connection;
                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    content = (byte[])dataReader.GetValue(dataReader.GetOrdinal("Content"));
                }
                dataReader.Close();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return content;
        }

        public List<Blob> SelectList(Guid masterGuid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "BlobSelectListByMasterGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@MasterGuid", masterGuid);
            return SelectList(command);
        }

    }
}

