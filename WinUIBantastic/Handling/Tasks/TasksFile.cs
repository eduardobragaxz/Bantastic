global using System.IO;

namespace WinUIBantastic.Handling.Tasks;

public static class TasksFile
{
    public static string FilePath { get => @"C:\Users\Public\TasksApp\TasksFile.txt"; }
    public static bool DoesFileExist { get => File.Exists(FilePath); }
    public static void CreateFile()
    {
        using StreamWriter streamWriter = new(FilePath, append: true);
    }

    public static async Task WriteToFile(string line)
    {
        using StreamWriter streamWriter = new(FilePath, append: true);
        await streamWriter.WriteLineAsync(line);
    }

    //Deletes the file when the user clicks on "delete file"
    public static void DeleteFileForGood()
    {
        TasksData.ToDoList.Clear();
        TasksData.InProgressList.Clear();
        TasksData.DoneList.Clear();
        File.Delete(FilePath);
    }
    //Deletes the file when syncing lists
    public static void DeleteFile()
    {
        File.Delete(FilePath);
    }
}