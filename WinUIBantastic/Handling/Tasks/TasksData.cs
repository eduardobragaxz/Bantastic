namespace WinUIBantastic.Handling.Tasks;

public record TasksData
{
    public static ObservableCollection<string> ToDoList { get; set; } = new();
    public static ObservableCollection<string> InProgressList { get; set; } = new();
    public static ObservableCollection<string> DoneList { get; set; } = new();
}