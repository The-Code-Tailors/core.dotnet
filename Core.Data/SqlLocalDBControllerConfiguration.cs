using System;
using System.Configuration;
using System.Data.SqlClient;

namespace com.fabioscagliola.Core.Data
{
    public class SqlLocalDBControllerConfiguration : ControllerConfiguration
    {
        protected string path;

        /// <summary>
        /// Creates an instance of the SqlLocalDBControllerConfiguration class pointing to the specified database 
        /// </summary>
        /// <param name="path">The path to the database, relative to the "CommonApplicationData" folder</param>
        public SqlLocalDBControllerConfiguration(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// The path to the database, relative to the "CommonApplicationData" folder 
        /// </summary>
        public string Path { get { return path; } }

        public static SqlLocalDBControllerConfiguration GetDefault()
        {
            AppSettingsReader appSettingsReader = new AppSettingsReader();
            string path = (string)appSettingsReader.GetValue("com.fabioscagliola.Core.Data.SqlLocalDBControllerConfiguration.Path", typeof(string));
            SqlLocalDBControllerConfiguration configuration = new SqlLocalDBControllerConfiguration(path);
            return configuration;
        }

        public override SqlConnection GetConnection()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();

            connectionStringBuilder.AttachDBFilename = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), path);
            connectionStringBuilder.DataSource = "(LocalDB)\\MSSQLLocalDB";
            connectionStringBuilder.IntegratedSecurity = true;
            connectionStringBuilder.ConnectTimeout = 60;

            return new SqlConnection(connectionStringBuilder.ConnectionString);
        }

    }
}

