

namespace WinUIBantastic.Handling.Settings;

internal static class SettingsHandling
{
    public static void InitializeSettings()
    {
        if (SettingsFile.SettingsFileExists)
        {
            foreach (string line in File.ReadLines(SettingsFile.FilePath))
            {
                switch (line)
                {
                    case "THEME1": SettingsData.AppsTheme = "THEME1"; break;
                    case "THEME2": SettingsData.AppsTheme = "THEME2"; break;
                    case "MICA0": SettingsData.MicaOn = false; break;
                    case "MICA1": SettingsData.MicaOn = true; break;
                }
            }
        }
        else
        {
            if (Directory.Exists(SettingsFile.FolderPath))
            {
                using StreamWriter streamWriter = new(SettingsFile.FilePath, append: true);
                streamWriter.WriteLine("THEME0");
                streamWriter.WriteLine("MICA0");
            }
            else
            {
                SettingsFile.CreateFolder();
                InitializeSettings();
            }
        }
    }

    public static void WriteSettingsToFile()
    {
        using StreamWriter streamWriter = new(SettingsFile.FilePath, append: true);
        streamWriter.WriteLine(SettingsData.AppsTheme);
        switch(SettingsData.MicaOn)
        {
            case true: streamWriter.WriteLine("MICA1"); break;
            case false: streamWriter.WriteLine("MICA0"); break;
        }
    }
    public static void SetTheme(string applicationTheme, bool micaOn)
    {
        SettingsFile.DeleteSettings();

        using StreamWriter streamWriter = new(SettingsFile.FilePath, append: true);

        switch(applicationTheme)
        {
            case "THEME1":
                SettingsData.AppsTheme = "THEME1";
                streamWriter.WriteLine("THEME1");
                break;
            case "THEME2":
                SettingsData.AppsTheme = "THEME2";
                streamWriter.WriteLine("THEME2");
                break;
            default:
                break;
        }

        switch(micaOn)
        {
            case true:
                streamWriter.WriteLine("MICA1");
                SettingsData.MicaOn = true;
                break;
            case false:
                streamWriter.WriteLine("MICA0");
                SettingsData.MicaOn = false;
                break;
        }
    }
}
