using Microsoft.Extensions.Configuration;

#nullable disable

namespace com.fabioscagliola.Core.Data;

public class Settings
{
    public class EntitySettings
    {
        public ControllerType ControllerType { get; set; }
    }

    public class SqlControllerConfigurationSettings
    {
        public string Hostname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public bool EnableDiagnosticMode { get; set; }
    }

    public class SqlIntegratedSecurityControllerConfigurationSettings
    {
        public string Hostname { get; set; }
        public string Database { get; set; }
        public bool EnableDiagnosticMode { get; set; }
    }

    public class SqlLocalDBControllerConfigurationSettings
    {
        public string Path { get; set; }
    }

    public class MailerSettings
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public bool SmtpEnableSsl { get; set; }
    }

    public class XmlControllerConfigurationSettings
    {
        public string Path { get; set; }
    }

    private static Settings instance;

    public static Settings Instance
    {
        get
        {
            if (instance != null) return instance;
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
            instance = configuration.GetRequiredSection(nameof(Settings)).Get<Settings>();
            return instance ?? throw new ApplicationException("Something went wrong while initializing the settings.");
        }
    }

    public EntitySettings Entity { get; set; }
    public SqlControllerConfigurationSettings SqlControllerConfiguration { get; set; }
    public SqlIntegratedSecurityControllerConfigurationSettings SqlIntegratedSecurityControllerConfiguration { get; set; }
    public SqlLocalDBControllerConfigurationSettings SqlLocalDBControllerConfiguration { get; set; }
    public MailerSettings Mailer { get; set; }
    public XmlControllerConfigurationSettings XmlControllerConfiguration { get; set; }
}