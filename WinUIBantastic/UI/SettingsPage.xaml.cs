using WinUIBantastic.Handling.Settings;

namespace WinUIBantastic.UI;

public sealed partial class SettingsPage : Page
{
    private string FileLocation { get => SettingsData.FileLocation; }
    private bool MicaOn { get => SettingsData.MicaOn; }
    private bool MicaChecked { get => (bool)CheckMica.IsChecked; set => _ = value; }
    private ContentDialog contentDialog;

    public SettingsPage()
    {
        InitializeComponent();

        switch(SettingsData.AppsTheme)
        {
            case "THEME1": RBLight.IsChecked = true; break;
            case "THEME2": RBDark.IsChecked = true; break;
        }

        switch(SettingsData.MicaOn)
        {
            case true: CheckMica.IsChecked = true; break;
            case false: CheckMica.IsChecked = false; break;
        }
    }

    private void BtnBackButton_Click(object sender, RoutedEventArgs e) => Frame.GoBack(new DrillInNavigationTransitionInfo());
    private void RBThemes_SelectionChanged(object sender, SelectionChangedEventArgs e) => BtnSaveSettings.IsEnabled = true;
    private void BtnSaveSettings_Click(object sender, RoutedEventArgs e) => SaveSettings();
    private async void SaveSettings()
    {
        string selectedTheme = "";

        if ((bool)RBLight.IsChecked)
        {
            selectedTheme = "THEME1";
        }
        else if ((bool)RBDark.IsChecked)
        {
            selectedTheme = "THEME2";
        }

        SettingsHandling.SetTheme(selectedTheme, MicaChecked);
        BtnSaveSettings.IsEnabled = false;
        await ShowDialog();
    }
    private async Task ShowDialog()
    {
        contentDialog = new()
        {
            XamlRoot = XamlRoot,
            Title = "Now just restart the app!",
            Content = "The changes have been made to the file. Just close and open the app to actually see them",
            CloseButtonText = "Ok"
        };

        await contentDialog.ShowAsync();
    }

    private void CheckMica_Checked(object sender, RoutedEventArgs e)
    {
        BtnSaveSettings.IsEnabled = true;
    }

    private void CheckMica_Unchecked(object sender, RoutedEventArgs e)
    {
        BtnSaveSettings.IsEnabled = true;
    }
}