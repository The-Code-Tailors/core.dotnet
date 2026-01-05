using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;

namespace com.fabioscagliola.Core.DataAccess
{
    public abstract class DataAccessSqlController<EntityType> : SqlController<EntityType, long> where EntityType : DataAccessEntity, new()
    {
        public virtual string NAMESPACE
        {
            get { return "http://fabioscagliola.com/Core/1"; }
        }

        protected string selectVersionHistoryCommandText;
        protected string selectVersionHistoryItemCommandText;
        //protected string undeleteCommandText;

        public DataAccessSqlController(ControllerConfiguration configuration, EntityType entity, string deleteCommandText, string insertCommandText, string selectCommandText, string selectListCommandText, string selectVersionHistoryCommandText, string updateCommandText)
            : this(configuration, entity, deleteCommandText, insertCommandText, selectCommandText, selectListCommandText, selectVersionHistoryCommandText, null, updateCommandText)
        { }

        public DataAccessSqlController(ControllerConfiguration configuration, EntityType entity, string deleteCommandText, string insertCommandText, string selectCommandText, string selectListCommandText, string selectVersionHistoryCommandText, string selectVersionHistoryItemCommandText, string updateCommandText)
            : base(configuration, entity, deleteCommandText, insertCommandText, selectCommandText, selectListCommandText, updateCommandText)
        {
            this.selectVersionHistoryCommandText = selectVersionHistoryCommandText;
            this.selectVersionHistoryItemCommandText = selectVersionHistoryItemCommandText;
        }

        //public DataAccessSqlController(ControllerConfiguration configuration, EntityType entity, string deleteCommandText, string insertCommandText, string selectCommandText, string selectListCommandText, string selectVersionHistoryCommandText, string selectVersionHistoryItemCommandText, string undeleteCommandText, string updateCommandText)
        //    : this(configuration, entity, deleteCommandText, insertCommandText, selectCommandText, selectListCommandText, selectVersionHistoryCommandText, selectVersionHistoryItemCommandText, updateCommandText)
        //{
        //    this.undeleteCommandText = undeleteCommandText;
        //}


        protected override void DoParameters(SqlCommand command)
        {
            // Do nothing 
            return;
        }

        protected override void Read(SqlDataReader dataReader, ref EntityType entity)
        {
            Type type = System.Type.GetType(dataReader.GetString(dataReader.GetOrdinal("DataType")));
            entity = (EntityType)Util.Deserialize(dataReader.GetString(dataReader.GetOrdinal("Data")), type);
            entity.Id = dataReader.GetInt64(dataReader.GetOrdinal("Id"));

            if (dataReader.ContainsColumn("SocialCount1"))
            {
                entity.SocialCount1 = dataReader.GetInt32(dataReader.GetOrdinal("SocialCount1"));
            }

            if (dataReader.ContainsColumn("SocialCount2"))
            {
                entity.SocialCount2 = dataReader.GetInt32(dataReader.GetOrdinal("SocialCount2"));
            }

            if (dataReader.ContainsColumn("SocialCount3"))
            {
                entity.SocialCount3 = dataReader.GetInt32(dataReader.GetOrdinal("SocialCount3"));
            }

            if (dataReader.ContainsColumn("SocialCount4"))
            {
                entity.SocialCount4 = dataReader.GetInt32(dataReader.GetOrdinal("SocialCount4"));
            }
        }


        public sealed override void Delete()
        {
            DeleteInternal(false);
        }

        public sealed override void Delete(bool permanently)
        {
            DeleteInternal(permanently);
        }

        public sealed override void Delete(SqlTransaction transaction)
        {
            DeleteInternal(transaction, false);
        }

        public sealed override void Delete(SqlTransaction transaction, bool permanently)
        {
            DeleteInternal(transaction, permanently);
        }

        protected virtual void DeleteInternal(bool permanently)
        {
            lock (this)
            {
                SqlConnection connection = GetConnection();
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    DeleteInternal(transaction, permanently);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        protected virtual void DeleteInternal(SqlTransaction transaction, bool permanently)
        {
            SqlConnection connection = transaction.Connection;
            SqlCommand command = connection.CreateCommand();
            command.CommandText = deleteCommandText;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@DeleteDate", entity.DeleteDate);
            command.Parameters.AddWithValue("@DeleteUserId", entity.DeleteUserId);
            command.Parameters.AddWithValue("@Data", Util.Serialize(entity));
            //command.Parameters.AddWithValue("@DataType", entity.DataType);
            command.Parameters.AddWithValue("@Permanently", permanently);
            command.Transaction = transaction;
            command.ExecuteNonQuery();
        }


        public sealed override void Update()
        {
            if (entity.Id == 0)
            {
                InsertInternal();
            }
            else
            {
                UpdateInternal();
            }
        }

        public sealed override void Update(SqlTransaction transaction)
        {
            if (entity.Id == 0)
            {
                InsertInternal(transaction);
            }
            else
            {
                UpdateInternal(transaction);
            }
        }

        protected virtual void InsertInternal()
        {
            lock (this)
            {
                SqlConnection connection = GetConnection();
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    InsertInternal(transaction);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        protected virtual void InsertInternal(SqlTransaction transaction)
        {
            SqlConnection connection = transaction.Connection;
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = insertCommandText;
                command.CommandType = CommandType.StoredProcedure;
                DoParameters(command);
                command.Parameters.AddWithValue("@Guid", entity.Guid);
                command.Parameters.AddWithValue("@InsertDate", entity.InsertDate);
                command.Parameters.AddWithValue("@InsertUserId", entity.InsertUserId);
                command.Parameters.AddWithValue("@Data", Util.Serialize(entity));
                command.Parameters.AddWithValue("@DataType", entity.DataType);
                command.Parameters.Add(new SqlParameter() { ParameterName = "@Id", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.BigInt });
                command.Transaction = transaction;
                command.ExecuteNonQuery();
                entity.Id = (long)command.Parameters["@Id"].Value;
            }
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = updateCommandText;
                command.CommandType = CommandType.StoredProcedure;
                DoParameters(command);
                command.Parameters.AddWithValue("@UpdateDate", DBNull.Value);
                command.Parameters.AddWithValue("@UpdateUserId", DBNull.Value);
                command.Parameters.AddWithValue("@Data", Util.Serialize(entity));
                //command.Parameters.AddWithValue("@DataType", entity.DataType);
                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@DoVersioning", false);
                command.Transaction = transaction;
                command.ExecuteNonQuery();
            }
        }

        protected virtual void UpdateInternal()
        {
            lock (this)
            {
                SqlConnection connection = GetConnection();
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    UpdateInternal(transaction);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        protected virtual void UpdateInternal(SqlTransaction transaction)
        {
            SqlConnection connection = transaction.Connection;
            SqlCommand command = connection.CreateCommand();
            command.CommandText = updateCommandText;
            command.CommandType = CommandType.StoredProcedure;
            DoParameters(command);
            command.Parameters.AddWithValue("@UpdateDate", entity.UpdateDate);
            command.Parameters.AddWithValue("@UpdateUserId", entity.UpdateUserId);
            command.Parameters.AddWithValue("@Data", Util.Serialize(entity));
            //command.Parameters.AddWithValue("@DataType", entity.DataType);
            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@DoVersioning", true);
            command.Transaction = transaction;
            command.ExecuteNonQuery();
        }


        public List<EntityType> SelectVersionHistory()
        {
            SqlCommand command = new SqlCommand() { CommandText = selectVersionHistoryCommandText, CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Id", entity.Id);
            return SelectList(command);
        }

        /// <summary>
        /// Returns an item from the version history 
        /// </summary>
        /// <param name="itemId">The identifier of the item in the version history (i.e. the value of the "Id" column)</param>
        /// <returns></returns>
        public EntityType SelectVersionHistoryItem(long itemId)
        {
            if (selectVersionHistoryItemCommandText == null)
            {
                throw new DataAccessException("'selectVersionHistoryItemCommandText' is null!");
            }

            SqlCommand command = new SqlCommand() { CommandText = selectVersionHistoryItemCommandText, CommandType = CommandType.StoredProcedure };
            long entityId = entity.Id;
            command.Parameters.AddWithValue("@Id", entityId);
            command.Parameters.AddWithValue("@ItemId", itemId);
            EntityType temp = Select(command);
            temp.Id = entityId;
            return temp;
        }

        public override List<EntityType> SelectList(SqlCommand command)
        {
            Inject(command);
            return base.SelectList(command);
        }

        //public EntityType SelectByAttribute(string attributeKey, string attributeValue)
        //{
        //    SqlCommand command = new SqlCommand() { CommandText = string.Format("SELECT * FROM [User] WHERE Data.query('/BYOMusicUser/Attributes/KeyValuePairOfStringObject[Key=\"{0}\"]/Value').value('.', 'varchar(max)') = @AttributeValue AND [DeleteDate] IS NULL AND VersioningId IS NULL", attributeKey), CommandType = CommandType.Text };
        //    command.Parameters.AddWithValue("@AttributeValue", attributeValue);
        //    Inject(command);
        //    return base.Select(command);
        //}

        protected override EntityType Select(SqlCommand command)
        {
            Inject(command);
            return base.Select(command);
        }

        protected virtual void Inject(SqlCommand command)
        {
            if (command.CommandType == CommandType.Text)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("SELECT *");
                stringBuilder.Append(", (SELECT [dbo].[BlobCount]([");
                stringBuilder.Append(typeof(EntityType).Name);
                stringBuilder.Append("].[Guid])) [SocialCount1]");
                stringBuilder.Append(", (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [");
                stringBuilder.Append(typeof(EntityType).Name);
                stringBuilder.Append("].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]");
                stringBuilder.Append(", (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [");
                stringBuilder.Append(typeof(EntityType).Name);
                stringBuilder.Append("].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]");
                stringBuilder.Append(", (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [");
                stringBuilder.Append(typeof(EntityType).Name);
                stringBuilder.Append("].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]");
                stringBuilder.Append(" FROM");
                command.CommandText = command.CommandText.Replace("SELECT * FROM", stringBuilder.ToString());
            }
        }


        //public void Undelete()
        //{
        //    UndeleteInternal();
        //}

        //public void Undelete(SqlTransaction transaction)
        //{
        //    UndeleteInternal(transaction);
        //}

        //protected virtual void UndeleteInternal()
        //{
        //    lock (this)
        //    {
        //        SqlConnection connection = GetConnection();
        //        connection.Open();
        //        SqlTransaction transaction = connection.BeginTransaction();
        //        try
        //        {
        //            UndeleteInternal(transaction);
        //            transaction.Commit();
        //        }
        //        catch
        //        {
        //            transaction.Rollback();
        //            throw;
        //        }
        //        finally
        //        {
        //            connection.Close();
        //            connection.Dispose();
        //        }
        //    }
        //}

        //protected virtual void UndeleteInternal(SqlTransaction transaction)
        //{
        //    if (undeleteCommandText == null)
        //    {
        //        throw new DataAccessException("'undeleteCommandText' is null!");
        //    }

        //    SqlConnection connection = transaction.Connection;
        //    SqlCommand command = connection.CreateCommand();
        //    command.CommandText = undeleteCommandText;
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.Parameters.AddWithValue("@Id", entity.Id);
        //    //command.Parameters.AddWithValue("@UndeleteDate", entity.UndeleteDate);
        //    //command.Parameters.AddWithValue("@UndeleteUserId", entity.UndeleteUserId);
        //    command.Parameters.AddWithValue("@Data", Util.Serialize(entity));
        //    command.Transaction = transaction;
        //    command.ExecuteNonQuery();
        //}

    }
}

