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
            return new XmlControllerConfiguration(Settings.Instance.XmlControllerConfiguration.Path);
        }

    }
}

