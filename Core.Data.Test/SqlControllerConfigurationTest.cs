using com.fabioscagliola.Core.Data;
using NUnit.Framework;

namespace Core.Data.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var sqlControllerConfiguration = SqlControllerConfiguration.GetDefault();
        Assert.That(sqlControllerConfiguration.Hostname, Is.EqualTo("Hostname"));
        Assert.That(sqlControllerConfiguration.Username, Is.EqualTo("Username"));
        Assert.That(sqlControllerConfiguration.Password, Is.EqualTo("Password"));
        Assert.That(sqlControllerConfiguration.Database, Is.EqualTo("Database"));
        Assert.That(sqlControllerConfiguration.EnableDiagnosticMode, Is.True);
        Assert.Pass();
    }
}