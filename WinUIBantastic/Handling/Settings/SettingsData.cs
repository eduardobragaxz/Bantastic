
namespace WinUIBantastic.Handling.Settings;

internal record SettingsData
{
    public static string FileLocation { get => SettingsFile.FilePath; }
    public static string AppsTheme { get; set; }
    public static bool MicaOn { get; set; }
}