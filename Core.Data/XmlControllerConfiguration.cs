using System.Configuration;

namespace com.fabioscagliola.Core.Data
{
    public class XmlControllerConfiguration
    {
        protected string path;

        public XmlControllerConfiguration(string path)
        {
            this.path = path;
        }

        public string Path { get { return path; } }

        public static XmlControllerConfiguration GetDefault()
        {
            AppSettingsReader appSettingsReader = new AppSettingsReader();
            string path = (string)appSettingsReader.GetValue("com.fabioscagliola.Core.Data.XmlControllerConfiguration.Path", typeof(string));
            return new XmlControllerConfiguration(path);
        }

    }
}

