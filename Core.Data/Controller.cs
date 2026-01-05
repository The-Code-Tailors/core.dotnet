using System;
using System.Configuration;
using System.Linq;

namespace com.fabioscagliola.Core.Data
{
    public abstract class Controller
    {
        public static ControllerType Type
        {
            get
            {
                ControllerType controllerType = ControllerType.Sql;

                AppSettingsReader appSettingsReader = new AppSettingsReader();

                const string CONTROLLERTYPE = "com.fabioscagliola.Core.Data.Entity.ControllerType";

                if (ConfigurationManager.AppSettings.AllKeys.Contains(CONTROLLERTYPE))
                {
                    controllerType = (ControllerType)Enum.Parse(typeof(ControllerType), (string)appSettingsReader.GetValue(CONTROLLERTYPE, typeof(string)));
                }

                return controllerType;
            }
        }

        public abstract void Delete();

        public abstract void Update();

        //public abstract ... Select(int id);

    }
}

