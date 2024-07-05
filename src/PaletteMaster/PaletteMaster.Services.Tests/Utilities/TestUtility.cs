namespace PaletteMaster.Services.Tests.Utilities;

public static class TestUtility
{
    public static string GetSamplePath(string fileName)
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "Samples", fileName);
    }
}