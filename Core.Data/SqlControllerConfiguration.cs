using System;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;

namespace com.fabioscagliola.Core.Data
{
    public class SqlControllerConfiguration : ControllerConfiguration
    {
        protected string hostname;
        protected string username;
        protected string password;
        protected string database;

        public SqlControllerConfiguration(string hostname, string username, string password, string database)
        {
            this.hostname = hostname;
            this.username = username;
            this.password = password;
            this.database = database;
        }

        public string Hostname { get { return hostname; } }

        public string Username { get { return username; } }

        public string Password { get { return password; } }

        public string Database { get { return database; } }

        public bool EnableDiagnosticMode { get; set; }

        public static SqlControllerConfiguration GetDefault()
        {
            var hostname = Settings.Instance.SqlControllerConfiguration.Hostname;
            var username = Settings.Instance.SqlControllerConfiguration.Username;
            var password = Settings.Instance.SqlControllerConfiguration.Password;
            var database = Settings.Instance.SqlControllerConfiguration.Database;
            var configuration = new SqlControllerConfiguration(hostname, username, password, database);

            configuration.EnableDiagnosticMode = Settings.Instance.SqlControllerConfiguration.EnableDiagnosticMode;

            return configuration;
        }

        public override SqlConnection GetConnection()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();

            connectionStringBuilder.DataSource = Hostname;
            connectionStringBuilder.InitialCatalog = Database;
            connectionStringBuilder.Password = Password;
            connectionStringBuilder.UserID = Username;

            if (EnableDiagnosticMode)
            {
                File.AppendAllText(Path.Combine(Path.GetTempPath(), string.Format("{0}.log", Database)), Environment.StackTrace + Environment.NewLine + Environment.NewLine);
            }

            return new SqlConnection(connectionStringBuilder.ConnectionString);
        }

    }
}

