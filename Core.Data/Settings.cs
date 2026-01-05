using Microsoft.Extensions.Configuration;

#nullable disable

namespace com.fabioscagliola.Core.Data;

public class Settings
{
    public class EntitySettings
    {
        public ControllerType ControllerType { get; init; }
    }

    public class SqlControllerConfigurationSettings
    {
        public string Hostname { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public string Database { get; init; }
        public bool EnableDiagnosticMode { get; init; }
    }

    public class SqlIntegratedSecurityControllerConfigurationSettings
    {
        public string Hostname { get; init; }
        public string Database { get; init; }
        public bool EnableDiagnosticMode { get; init; }
    }

    public class SqlLocalDBControllerConfigurationSettings
    {
        public string Path { get; init; }
    }

    public class XmlControllerConfigurationSettings
    {
        public string Path { get; init; }
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
    public XmlControllerConfigurationSettings XmlControllerConfiguration { get; set; }
}