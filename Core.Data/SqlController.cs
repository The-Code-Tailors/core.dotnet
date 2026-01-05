using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace com.fabioscagliola.Core.Data
{
    public abstract class SqlController : Controller
    {
        protected ControllerConfiguration configuration;

        public SqlController(ControllerConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public virtual SqlConnection GetConnection()
        {
            return configuration.GetConnection();
        }

        public abstract void Delete(bool permanently);

        public abstract void Delete(SqlTransaction transaction);

        public abstract void Delete(SqlTransaction transaction, bool permanently);

        public abstract void Update(SqlTransaction transaction);
    }

    public abstract class SqlController<EntityType, EntityIdType> : SqlController where EntityType : Entity<EntityIdType>, new()
    {
        protected EntityType entity;

        protected string deleteCommandText;
        protected string insertCommandText;
        protected string selectCommandText;
        protected string selectListCommandText;
        protected string updateCommandText;

        public SqlController(ControllerConfiguration configuration, EntityType entity, string deleteCommandText, string insertCommandText, string selectCommandText, string selectListCommandText, string updateCommandText)
            : base(configuration)
        {
            this.entity = entity;
            this.deleteCommandText = deleteCommandText;
            this.insertCommandText = insertCommandText;
            this.selectCommandText = selectCommandText;
            this.selectListCommandText = selectListCommandText;
            this.updateCommandText = updateCommandText;
        }

        protected abstract void DoParameters(SqlCommand command);

        protected abstract void Read(SqlDataReader dataReader, ref EntityType entity);

        protected object GetValueOrDBNull(object value)
        {
            return value ?? DBNull.Value;
        }

        public override void Delete()
        {
            SqlConnection connection = GetConnection();
            try
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = deleteCommandText;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", entity.Id);
                connection.Open();
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        public override void Update()
        {
            SqlConnection connection = GetConnection();
            try
            {
                if (entity.Id.Equals(default(EntityIdType)))  // Insert
                {
                    lock (this)
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = insertCommandText;
                        command.CommandType = CommandType.StoredProcedure;
                        DoParameters(command);
                        command.Parameters.AddWithValue("@Id", entity.Id).Direction = ParameterDirection.Output;  // TODO: [Fabio] ..... 
                        connection.Open();
                        command.ExecuteNonQuery();
                        entity.Id = (EntityIdType)command.Parameters["@Id"].Value;
                    }
                }
                else  // Update
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = updateCommandText;
                    command.CommandType = CommandType.StoredProcedure;
                    DoParameters(command);
                    command.Parameters.AddWithValue("@Id", entity.Id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        public virtual EntityType Select(EntityIdType id)
        {
            SqlCommand command = new SqlCommand() { CommandText = selectCommandText, CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Id", id);
            return Select(command);
        }

        protected virtual EntityType Select(SqlCommand command)
        {
            SqlConnection connection = GetConnection();
            try
            {
                command.Connection = connection;
                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    Read(dataReader, ref entity);
                }
                dataReader.Close();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return entity;
        }

        public virtual List<EntityType> SelectList()
        {
            SqlCommand command = new SqlCommand() { CommandText = selectListCommandText, CommandType = CommandType.StoredProcedure };
            return SelectList(command);
        }

        public virtual List<EntityType> SelectList(SqlCommand command)
        {
            List<EntityType> list = new List<EntityType>();
            SqlConnection connection = GetConnection();
            try
            {
                command.Connection = connection;
                connection.Open();
                DoList(command, list);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return list;
        }

        public virtual List<EntityType> SelectList(SqlCommand command, SqlTransaction transaction)
        {
            List<EntityType> list = new List<EntityType>();
            command.Connection = transaction.Connection;
            command.Transaction = transaction;
            DoList(command, list);
            return list;
        }

        protected void DoList(SqlCommand command, List<EntityType> list)
        {
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                EntityType entity = new EntityType();
                Read(dataReader, ref entity);
                list.Add(entity);
            }
            dataReader.Close();
        }

    }
}

