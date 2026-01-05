using System;
using System.Configuration;
using Microsoft.Data.SqlClient;

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
            var path = Settings.Instance.SqlLocalDBControllerConfiguration.Path;
            var configuration = new SqlLocalDBControllerConfiguration(path);
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

