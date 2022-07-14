
namespace WinUIBantastic.Handling.Settings;

internal static class SettingsFile
{
    public static string FolderPath { get => @"C:\Users\Public\TasksApp\"; }
    public static string FilePath { get => $@"{FolderPath}TasksSettings.txt"; }
    public static bool SettingsFileExists { get => File.Exists(FilePath); }

    public static void CreateFolder()
    {
        Directory.CreateDirectory(FolderPath);
    }
    public static void DeleteSettings()
    {
        File.Delete(FilePath);
    }
}
