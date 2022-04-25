namespace ClarityEmailer.API.Logging;

public static class LogToFile
{
    public static string FilePath { get; set; }

    public static async Task WriteEntryAsync(string jsonEntry)
    {
        if (!File.Exists(FilePath))
        {
            File.Create(FilePath);
        }
        await File.WriteAllTextAsync(FilePath, jsonEntry + $",\r\n");
    }
}