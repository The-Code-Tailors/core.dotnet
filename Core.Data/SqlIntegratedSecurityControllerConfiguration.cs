using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace com.fabioscagliola.Core.Data
{
    public class SqlIntegratedSecurityControllerConfiguration : ControllerConfiguration
    {
        protected string hostname;
        protected string database;

        public SqlIntegratedSecurityControllerConfiguration(string hostname, string database)
        {
            this.hostname = hostname;
            this.database = database;
        }

        public string Hostname { get { return hostname; } }

        public string Database { get { return database; } }

        public bool EnableDiagnosticMode { get; set; }

        public static SqlIntegratedSecurityControllerConfiguration GetDefault()
        {
            AppSettingsReader appSettingsReader = new AppSettingsReader();
            string hostname = (string)appSettingsReader.GetValue("com.fabioscagliola.Core.Data.SqlIntegratedSecurityControllerConfiguration.Hostname", typeof(string));
            string database = (string)appSettingsReader.GetValue("com.fabioscagliola.Core.Data.SqlIntegratedSecurityControllerConfiguration.Database", typeof(string));
            SqlIntegratedSecurityControllerConfiguration configuration = new SqlIntegratedSecurityControllerConfiguration(hostname, database);

            const string ENABLEDIAGNOSTICMODE = "com.fabioscagliola.Core.Data.SqlIntegratedSecurityControllerConfiguration.EnableDiagnosticMode";

            if (ConfigurationManager.AppSettings.AllKeys.Contains(ENABLEDIAGNOSTICMODE))
            {
                configuration.EnableDiagnosticMode = (bool)appSettingsReader.GetValue(ENABLEDIAGNOSTICMODE, typeof(bool));
            }

            return configuration;
        }

        public override SqlConnection GetConnection()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();

            connectionStringBuilder.DataSource = Hostname;
            connectionStringBuilder.InitialCatalog = Database;
            connectionStringBuilder.IntegratedSecurity = true;

            if (EnableDiagnosticMode)
            {
                File.AppendAllText(Path.Combine(Path.GetTempPath(), string.Format("{0}.log", Database)), Environment.StackTrace + Environment.NewLine + Environment.NewLine);
            }

            return new SqlConnection(connectionStringBuilder.ConnectionString);
        }

    }
}

