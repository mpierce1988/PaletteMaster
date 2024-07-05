namespace PaletteMaster.Services.ImageSharp.Tests.Utilities;

public static class TestUtility
{
    public static string GetSamplePath(string fileName)
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "Samples", fileName);
    }
}