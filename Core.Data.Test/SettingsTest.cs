using com.fabioscagliola.Core.Data;
using NUnit.Framework;

namespace Core.Data.Test;

public class SettingsTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestSettings()
    {
        Assert.That(Settings.Instance.Entity.ControllerType, Is.EqualTo(ControllerType.Sql));
        Assert.That(Settings.Instance.Mailer.SmtpHost, Is.EqualTo("SmtpHost"));
        Assert.That(Settings.Instance.Mailer.SmtpPort, Is.EqualTo(25));
        Assert.That(Settings.Instance.Mailer.SmtpUsername, Is.EqualTo("SmtpUsername"));
        Assert.That(Settings.Instance.Mailer.SmtpPassword, Is.EqualTo("SmtpPassword"));
        Assert.That(Settings.Instance.Mailer.SmtpEnableSsl, Is.True);
        Assert.That(Settings.Instance.SqlControllerConfiguration.Hostname, Is.EqualTo("Hostname"));
        Assert.That(Settings.Instance.SqlControllerConfiguration.Username, Is.EqualTo("Username"));
        Assert.That(Settings.Instance.SqlControllerConfiguration.Password, Is.EqualTo("Password"));
        Assert.That(Settings.Instance.SqlControllerConfiguration.Database, Is.EqualTo("Database"));
        Assert.That(Settings.Instance.SqlControllerConfiguration.EnableDiagnosticMode, Is.True);
        Assert.Pass();
    }
}