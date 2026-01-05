using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using System.Text;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public class Dynamo : DynamicObject, IEditable
    {
        protected Dictionary<string, object> dictionary = new Dictionary<string, object>();
        protected FlexibleEntityInstance flexibleEntityInstance;

        /// <summary>
        /// Protected, used by the Select static method to create instances for existing objects 
        /// </summary>
        protected Dynamo(FlexibleEntityInstance flexibleEntityInstance)
        {
            this.flexibleEntityInstance = flexibleEntityInstance;
        }

        /// <summary>
        /// Public, used to create instances for non-existing objects 
        /// </summary>
        public Dynamo(long flexibleEntityId)
        {
            flexibleEntityInstance = new FlexibleEntityInstance();
            flexibleEntityInstance.FlexibleEntityId = flexibleEntityId;

            // Using system milieu to avoid contructor parameter 
            List<FlexibleColumn> flexibleColumnList = FlexibleColumn.SelectListByFlexibleEntityId(Milieu.SystemMilieu, flexibleEntityId);

            foreach (FlexibleColumn flexibleColumn in flexibleColumnList)
            {
                dictionary.Add(flexibleColumn.Name, null);
            }
        }

        public FlexibleEntityInstance FlexibleEntityInstance { get { return flexibleEntityInstance; } }

        public long Id { get { return flexibleEntityInstance.Id; } }
        public Guid Guid { get { return flexibleEntityInstance.Guid; } }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return dictionary.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (dictionary.ContainsKey(binder.Name))
            {
                dictionary[binder.Name] = value;
                return true;
            }
            return false;
        }

        public object this[string key]
        {
            get
            {
                return dictionary[key];
            }
            set
            {
                dictionary[key] = value;
            }
        }

        public static Dynamo Select(Milieu milieu, long flexibleEntityInstanceId)
        {
            FlexibleEntityInstance flexibleEntityInstance = FlexibleEntityInstance.Select(milieu, flexibleEntityInstanceId);
            Dynamo dynamo = new Dynamo(flexibleEntityInstance);
            foreach (FlexibleColumn flexibleColumn in FlexibleColumn.SelectListByFlexibleEntityId(milieu, flexibleEntityInstance.FlexibleEntityId))
            {
                dynamo.dictionary.Add(flexibleColumn.Name, flexibleEntityInstance.GetValue(milieu, flexibleColumn.Id));
            }
            return dynamo;
        }

        public static DataTable SelectDataTable(Milieu milieu, long flexibleEntityId)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(long));
            dataTable.Columns.Add("Guid", typeof(Guid));
            foreach (FlexibleColumn flexibleColumn in FlexibleColumn.SelectListByFlexibleEntityId(milieu, flexibleEntityId))
            {
                dataTable.Columns.Add(flexibleColumn.Name, Type.GetType(flexibleColumn.Type));
            }
            dataTable.Columns.Add("SocialCount1", typeof(int));
            dataTable.Columns.Add("SocialCount2", typeof(int));
            dataTable.Columns.Add("SocialCount3", typeof(int));
            dataTable.Columns.Add("SocialCount4", typeof(int));
            foreach (Dynamo dynamo in Dynamo.SelectList(milieu, flexibleEntityId))
            {
                const int I = 2;
                object[] values = new object[dynamo.dictionary.Count + I];
                values[0] = dynamo.Id;
                values[1] = dynamo.Guid;
                dynamo.dictionary.Values.CopyTo(values, I);
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static List<Dynamo> SelectList(Milieu milieu, long flexibleEntityId)
        {
            List<FlexibleColumn> flexibleColumnList = ((FlexibleColumnSqlController)new FlexibleColumn().GetDefaultController()).SelectListByFlexibleEntityId(flexibleEntityId);

            List<Dynamo> list = new List<Dynamo>();

            SqlController controller = (SqlController)new FlexibleEntityColumnInstance().GetDefaultController();
            SqlConnection connection = controller.GetConnection();
            SqlCommand command = connection.CreateCommand();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("set transaction isolation level read uncommitted; select Data, DataType, [Guid], ");
            foreach (FlexibleColumn flexibleColumn in flexibleColumnList)
            {
                stringBuilder.Append("[");
                stringBuilder.Append(flexibleColumn.Id);
                stringBuilder.Append("] [");
                stringBuilder.Append(flexibleColumn.Name);
                stringBuilder.Append("], ");
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            //stringBuilder.Append(", (SELECT COUNT(*) FROM [");
            //stringBuilder.Append(connection.Database);
            //stringBuilder.Append(".Blob].[dbo].[Blob] WHERE [MasterGuid] = b.[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount1], (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = b.[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2], (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = b.[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3], (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = b.[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]");
            stringBuilder.Append(", 0 [SocialCount1], 0 [SocialCount2], 0 [SocialCount3], 0 [SocialCount4]");
            stringBuilder.Append(" from (select cast(FlexibleEntityInstance.Data as nvarchar(max)) Data, FlexibleEntityInstance.DataType, FlexibleEntityInstance.[Guid], FlexibleEntityColumnInstance.FlexibleColumnId, cast(FlexibleEntityColumnInstance.XmlValue as nvarchar(max)) XmlValue from FlexibleEntityInstance join FlexibleEntityColumnInstance on FlexibleEntityInstance.Id = FlexibleEntityColumnInstance.FlexibleEntityInstanceId where FlexibleEntityInstance.FlexibleEntityId = ");
            stringBuilder.Append(flexibleEntityId);
            stringBuilder.Append(" and FlexibleEntityColumnInstance.DeleteDate is null and FlexibleEntityColumnInstance.VersioningId is null) a pivot (max(XmlValue) for FlexibleColumnId in (");
            foreach (FlexibleColumn flexibleColumn in flexibleColumnList)
            {
                stringBuilder.Append("[");
                stringBuilder.Append(flexibleColumn.Id);
                stringBuilder.Append("], ");
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            stringBuilder.Append(")) b");

            try
            {
                command.CommandText = stringBuilder.ToString();
                command.CommandType = CommandType.Text;
                command.Connection = connection;
                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    FlexibleEntityInstance flexibleEntityInstance = new FlexibleEntityInstance();

                    Type type = Type.GetType(dataReader.GetString(dataReader.GetOrdinal("DataType")));
                    flexibleEntityInstance = (FlexibleEntityInstance)Util.Deserialize(dataReader.GetString(dataReader.GetOrdinal("Data")), type);

                    Dynamo dynamo = new Dynamo(flexibleEntityInstance);

                    foreach (FlexibleColumn flexibleColumn in flexibleColumnList)
                    {
                        dynamo.dictionary[flexibleColumn.Name] = null;

                        int i = dataReader.GetOrdinal(flexibleColumn.Name);

                        if (!dataReader.IsDBNull(i))
                        {
                            string xmlValue = dataReader.GetString(i);

                            if (!string.IsNullOrWhiteSpace(xmlValue))
                            {
                                dynamo.dictionary[flexibleColumn.Name] = Util.Deserialize(xmlValue, Type.GetType(flexibleColumn.Type));
                            }
                        }
                    }

                    dynamo.dictionary["SocialCount1"] = dataReader.GetInt32(dataReader.FieldCount - 4);
                    dynamo.dictionary["SocialCount2"] = dataReader.GetInt32(dataReader.FieldCount - 3);
                    dynamo.dictionary["SocialCount3"] = dataReader.GetInt32(dataReader.FieldCount - 2);
                    dynamo.dictionary["SocialCount4"] = dataReader.GetInt32(dataReader.FieldCount - 1);

                    list.Add(dynamo);
                }
                dataReader.Close();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return list;
        }

        public void Update(Milieu milieu)
        {
            Update(milieu, null);
        }

        public void Update(Milieu milieu, SqlTransaction transaction)
        {
            if (transaction == null)
            {
                SqlController controller = (SqlController)new FlexibleEntityInstance().GetDefaultController();
                SqlConnection connection = controller.GetConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                try
                {
                    UpdateInternal(milieu, transaction, true);
                }
                finally
                {
                    transaction.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
            else
            {
                UpdateInternal(milieu, transaction, false);
            }
        }

        protected void UpdateInternal(Milieu milieu, SqlTransaction transaction, bool doCommitRollback)
        {
            try
            {
                if (flexibleEntityInstance.Id == 0)  // Insert 
                {
                    flexibleEntityInstance.Update(milieu, transaction);
                }

                foreach (FlexibleColumn flexibleColumn in FlexibleColumn.SelectListByFlexibleEntityId(milieu, flexibleEntityInstance.FlexibleEntityId))
                {
                    flexibleEntityInstance.SetValue(milieu, flexibleColumn.Id, dictionary[flexibleColumn.Name], transaction);
                }

                if (doCommitRollback)
                {
                    transaction.Commit();
                }
            }
            catch
            {
                if (doCommitRollback)
                {
                    transaction.Rollback();
                }
                throw;
            }
        }

        public void Delete(Milieu milieu, bool permanently)
        {
            Delete(milieu, null, permanently);
        }

        public void Delete(Milieu milieu, SqlTransaction transaction, bool permanently)
        {
            if (transaction == null)
            {
                SqlController controller = (SqlController)new FlexibleEntityInstance().GetDefaultController();
                SqlConnection connection = controller.GetConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                try
                {
                    DeleteInternal(milieu, transaction, permanently, true);
                }
                finally
                {
                    transaction.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
            else
            {
                DeleteInternal(milieu, transaction, permanently, false);
            }
        }

        protected void DeleteInternal(Milieu milieu, SqlTransaction transaction, bool permanently, bool doCommitRollback)
        {
            try
            {
                flexibleEntityInstance.Delete(milieu, transaction, permanently);

                if (doCommitRollback)
                {
                    transaction.Commit();
                }
            }
            catch
            {
                if (doCommitRollback)
                {
                    transaction.Rollback();
                }
                throw;
            }
        }

    }
}

