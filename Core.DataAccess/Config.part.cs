using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class Config
    {
        public string Comment { get; set; }

        private static List<Config> data;

        private static void ReloadData()
        {
            data = Config.SelectList(Milieu.SystemMilieu).OrderBy(x => x.Key).ToList();
        }

        static Config()
        {
            ReloadData();
        }

        public static Config Select(Milieu milieu, string key)
        {
            EnsureAuthorized(milieu, DataAccessFunction.ConfigSelect);
            Config config = data.Find(x => x.Key == key);
            if (config == null)  // Prevents NullReferenceException when checking "if (config.Id == 0)" 
            {
                config = new Config();
            }
            return config;
        }

        public override void Delete(Milieu milieu, SqlTransaction transaction, bool permanently)
        {
            base.Delete(milieu, transaction, permanently);

            ReloadData();
        }

        public override void Update(Milieu milieu, SqlTransaction transaction)
        {
            base.Update(milieu, transaction);

            ReloadData();
        }

        public static bool IsUserSignInLoggingEnabled
        {
            get
            {
                const string KEY = "IsUserSignInLoggingEnabled";
                Config config = Config.Select(Milieu.SystemMilieu, KEY);
                if (config.Id == 0)
                {
                    config.Key = KEY;
                    config.Value = false;
                    config.Update(Milieu.SystemMilieu);
                }
                return (bool)config.Value;
            }
        }

        public static string SendMailFrom
        {
            get
            {
                const string KEY = "SendMailFrom";
                Config config = Config.Select(Milieu.SystemMilieu, KEY);
                if (config.Id == 0)
                {
                    config.Key = KEY;
                    config.Value = "Fabio Scagliola <info@fabioscagliola.com>";
                    config.Update(Milieu.SystemMilieu);
                }
                return (string)config.Value;
            }
        }

    }
}

