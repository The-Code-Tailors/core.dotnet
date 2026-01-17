using com.fabioscagliola.Core.Presentation.Cropsize;
using NUnit.Framework;

namespace com.fabioscagliola.Core.Presentation.Test;

public class CropsizerTest
{
    private string downloads;

    [SetUp]
    public void Setup()
    {
        downloads = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads"
        );
    }

    [Test]
    [TestCase("https://fabioscagliola.com/images/the-shift.png", 1920, 1440, Cropsizer.Anchor.Bottom)]
    [TestCase("https://fabioscagliola.com/images/the-shift.png", 1440, 1920, Cropsizer.Anchor.Bottom)]
    public void Cropsize(string requestUri, int w, int h, Cropsizer.Anchor anchor)
    {
        var httpClient = new HttpClient();
        var source = httpClient.GetByteArrayAsync(requestUri).Result;

        var cropsizer = new Cropsizer();
        var target = cropsizer.Cropsize(source, w, h, anchor);

        var path = Path.Combine(downloads, $"{Guid.NewGuid()}.png");
        File.WriteAllBytes(path, target);

        Assert.Pass();
    }

    [Test]
    [TestCase("https://fabioscagliola.com/images/the-shift.png", 1920, 1440, true, 75)]
    [TestCase("https://fabioscagliola.com/images/the-shift.png", 1920, 1440, false, 75)]
    public void ResizeJpeg(string requestUri, int w, int h, bool preserve, int quality)
    {
        using var httpClient = new HttpClient();
        var source = httpClient.GetByteArrayAsync(requestUri).Result;

        var cropsizer = new Cropsizer();
        var target = cropsizer.ResizeJpeg(source, w, h, preserve, quality);

        var path = Path.Combine(downloads, $"{Guid.NewGuid()}.jpg");
        File.WriteAllBytes(path, target);

        Assert.Pass();
    }
}
