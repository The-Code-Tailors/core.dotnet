using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess
{
    /// <summary>
    /// Facilitates access to the configuration settings stored in the Config table 
    /// </summary>
    public class ConfigHelper
    {
        #region Singleton pattern implementation, using lazy initialization 

        protected ConfigHelper()
        {
            Reload();
        }

        protected static ConfigHelper instance = null;

        /// <summary>
        /// Singleton pattern implementation, using lazy initialization 
        /// </summary>
        public static ConfigHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigHelper();
                }

                return instance;
            }
        }

        // Refresh instead of Reload becouse in conflict with instance method
        public static void Refresh()
        {
            // invalidate the static instance, so next time will be re-initialized
            instance = null;
        }

        #endregion

        protected List<Config> configList;

        protected void Reload()
        {
            configList = Config.SelectList(Milieu.SystemMilieu);
        }

        /// <summary>
        /// The list of the configuration settings stored in the Config table 
        /// </summary>
        public List<Config> ConfigList
        {
            get
            {
                return configList;
            }
        }

        /// <summary>
        /// Gets the value of a configuration setting 
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="key">The key indentifying the configuration setting</param>
        public T GetValue<T>(string key)
        {
            T value = default(T);
            Config config = configList.Find(x => x.Key == key);
            if (config != null)
            {
                value = (T)config.Value;
            }
            return value;
        }

        /// <summary>
        /// Sets the value of a configuration setting 
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="key">The key indentifying the configuration setting</param>
        /// <param name="value">The value of the configuration setting</param>
        public void SetValue<T>(string key, T value)
        {
            Config config = configList.Find(x => x.Key == key);

            if (config == null)
            {
                config = new Config();
                config.Key = key;
            }

            if (value != null && !value.Equals(config.Value))
            {
                config.Value = value;
                config.Update(Milieu.SystemMilieu);
                Reload();
            }
        }

    }
}

