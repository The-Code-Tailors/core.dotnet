using com.fabioscagliola.Core.DataAccess.Social;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace com.fabioscagliola.Core.DataAccess
{
    public abstract class InitializationBase
    {
        protected const string ME = "scagliola@gmail.com";

        private static void Execute(SqlConnection connection, string sql)
        {
            Execute(connection, sql, null);
        }

        private static void Execute(SqlConnection connection, string sql, int? timeout)
        {
            string[] commandTextList = sql.Split(new string[] { "GO" + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim(Environment.NewLine.ToCharArray())).ToList().FindAll(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            try
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                if (timeout.HasValue)
                {
                    command.CommandTimeout = timeout.Value;
                }
                connection.Open();

                foreach (string commandText in commandTextList)
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }

            }
            finally
            {
                connection.Close();
            }
        }

        public static void Execute(string sql)
        {
            UserSqlController controller = (UserSqlController)new User().GetDefaultController();
            SqlConnection connection = controller.GetConnection();
            Execute(connection, sql);
        }

        public static void Execute(string sql, int timeout)
        {
            UserSqlController controller = (UserSqlController)new User().GetDefaultController();
            SqlConnection connection = controller.GetConnection();
            Execute(connection, sql, timeout);
        }

        public static void ExecuteOnBlob(string sql)
        {
            BlobSqlController controller = (BlobSqlController)new Blob().GetDefaultController();
            SqlConnection connection = controller.GetConnection();
            Execute(connection, sql);
        }

        public static bool IsDatabaseVoid()
        {
            bool result;
            UserSqlController controller = (UserSqlController)new User().GetDefaultController();
            SqlConnection connection = controller.GetConnection();
            try
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "select * from sys.tables where sys.tables.name = 'User'";
                command.CommandType = CommandType.Text;
                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                result = !dataReader.Read();
                dataReader.Close();
            }
            finally
            {
                connection.Close();
            }
            return result;
        }

    }
}

