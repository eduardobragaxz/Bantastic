global using System.Collections.ObjectModel;
global using Microsoft.UI.Xaml.Controls;
global using System.Threading.Tasks;
using WinUIBantastic.Handling.Tasks;
using System;

namespace WinUIBantastic.UI;

public sealed partial class MainPage : Page
{
    ObservableCollection<string> UITodoList { get => TasksData.ToDoList; }
    ObservableCollection<string> UIInProgress { get => TasksData.InProgressList; }
    ObservableCollection<string> UIDone { get => TasksData.DoneList; }
    readonly ContentDialog contentDialog = new();

    public MainPage()
    {
        InitializeComponent();

        if(!Directory.Exists(@"C:\Users\Public\TasksApp"))
        {
            Directory.CreateDirectory(@"C:\Users\Public\TasksApp");
        }

        TasksHandling.InitializeTasks();
    }

    private async void BtnAdd_Click(SplitButton sender, SplitButtonClickEventArgs args) => await AddTask();
    private async void BtnMove_Click(SplitButton sender, SplitButtonClickEventArgs args) => await MoveTask();
    private async void BtnErase_Click(object sender, RoutedEventArgs e) => await EraseTask();
    private async void BtnRefreshLists_Click(object sender, RoutedEventArgs e) => await RefreshAllLists();
    private void BtnDeleteFile_Click(object sender, RoutedEventArgs e) => DeleteFile();
    private void TxtTaskInput_TextChanged(object sender, TextChangedEventArgs e) => EnableAddButton();
    private void LVToDo_SelectionChanged(object sender, SelectionChangedEventArgs e) => EnableMoveAndEraseButtons();
    private void LVInProgress_SelectionChanged(object sender, SelectionChangedEventArgs e) => EnableMoveAndEraseButtons();
    private void LVDone_SelectionChanged(object sender, SelectionChangedEventArgs e) => EnableMoveAndEraseButtons();
    private async void LVTodoRefresh_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
    {
        using var RefreshCompletionDeferral = args.GetDeferral();
        await RefreshToDoList();
    }
    private async void LVInProgressRefresh_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
    {
        using var RefreshCompletionDeferral = args.GetDeferral();
        await RefreshInProgressList();
    }
    private async void LVDoneRefresh_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
    {
        using var RefreshCompletionDeferral = args.GetDeferral();
        await RefreshDoneList();
    }
    private async void AddButtonListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TxtTaskInput is not null)
        {
            AddButtonFlyout.Hide();
            await AddTask();
        }
    }
    private async void MoveButtonListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LVToDo is not null && LVInProgress is not null && LVDone is not null) await MoveTask();
    }
    private async void TxtTaskInput_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (e.Key is Windows.System.VirtualKey.Enter) await AddTask();
    }
    private async Task AddTask()
    {
        if (AddFlyoutToDo.IsSelected)
        {
            if (!UITodoList.Contains(TxtTaskInput.Text))
            {
                string task = TxtTaskInput.Text;
                TxtTaskInput.Text = "";
                await TasksHandling.AddTask(task, '1');
            }
            else
            {
                TxtTaskInput.Text = "";
                await TaskAlreadyAddedMessage();
            }
        }
        else if (AddFlyoutInProgress.IsSelected)
        {
            if (!UIInProgress.Contains(TxtTaskInput.Text))
            {
                string task = TxtTaskInput.Text;
                TxtTaskInput.Text = "";
                await TasksHandling.AddTask(task, '2');

            }
            else
            {
                TxtTaskInput.Text = "";
                await TaskAlreadyAddedMessage();
            }
        }
        else if (AddFlyoutDone.IsSelected)
        {
            if (!UIDone.Contains(TxtTaskInput.Text))
            {
                string task = TxtTaskInput.Text;
                TxtTaskInput.Text = "";
                await TasksHandling.AddTask(task, '2');

            }
            else
            {
                TxtTaskInput.Text = "";
                await TaskAlreadyAddedMessage();
            }
        }

        UnselectFromLists();
    }
    private async Task MoveTask()
    {
        if (MoveFlyoutToDo.IsSelected)
        {
            while (LVInProgress.SelectedItems.Count > 0)
            {
                await TasksHandling.MoveTask(LVInProgress.SelectedItems, '1');
            }

            while (LVDone.SelectedItems.Count > 0)
            {
                await TasksHandling.MoveTask(LVDone.SelectedItems, '1');
            }
        }
        else if (MoveFlyoutInProgress.IsSelected)
        {
            while (LVToDo.SelectedItems.Count > 0)
            {
                await TasksHandling.MoveTask(LVToDo.SelectedItems, '2');
            }

            while (LVDone.SelectedItems.Count > 0)
            {
                await TasksHandling.MoveTask(LVDone.SelectedItems, '2');
            }
        }
        else
        {
            while (LVToDo.SelectedItems.Count > 0)
            {
                await TasksHandling.MoveTask(LVToDo.SelectedItems, '3');
            }

            while (LVInProgress.SelectedItems.Count > 0)
            {
                await TasksHandling.MoveTask(LVInProgress.SelectedItems, '3');
            }
        }

        DisableMoveAndEraseButtons();
    }
    private async Task EraseTask()
    {
        while (LVToDo.SelectedItems.Count > 0)
        {
            await TasksHandling.EraseTask(LVToDo.SelectedItems);
        }

        while (LVInProgress.SelectedItems.Count > 0)
        {
            await TasksHandling.EraseTask(LVInProgress.SelectedItems);
        }

        while (LVDone.SelectedItems.Count > 0)
        {
            await TasksHandling.EraseTask(LVDone.SelectedItems);
        }

        DisableMoveAndEraseButtons();
    }
    private static async Task RefreshAllLists()
    {
        await TasksHandling.RefreshAllLists();
    }
    private static void DeleteFile()
    {
        TasksFile.DeleteFileForGood();
    }
    private async Task TaskAlreadyAddedMessage()
    {
        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        contentDialog.XamlRoot = Content.XamlRoot;
        contentDialog.Title = "Task already added!";
        contentDialog.CloseButtonText = "Ok";

        await contentDialog.ShowAsync();

    }
    private void EnableMoveAndEraseButtons()
    {
        BtnMove.IsEnabled = true;
        BtnErase.IsEnabled = true;
    }
    private void DisableMoveAndEraseButtons()
    {
        BtnMove.IsEnabled = false;
        BtnErase.IsEnabled = false;
        MoveButtonFlyout.Hide();
    }
    private void EnableAddButton()
    {
        if (TxtTaskInput.Text is not "")
        {
            BtnAdd.IsEnabled = true;
        }
        else
        {
            BtnAdd.IsEnabled = false;
        }
    }
    private void UnselectFromLists()
    {
        LVToDo.SelectedItems.Clear();
        LVInProgress.SelectedItems.Clear();
        LVDone.SelectedItems.Clear();
    }
    private static async Task RefreshToDoList()
    {
        await TasksHandling.RefreshToDoList();
    }
    private static async Task RefreshInProgressList()
    {
        await TasksHandling.RefreshInProgressList();
    }
    private static async Task RefreshDoneList()
    {
        await TasksHandling.RefreshDoneList();
    }
    private void BtnSettings_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(SettingsPage), null, new DrillInNavigationTransitionInfo());
    }
}
