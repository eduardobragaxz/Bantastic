using System.Collections.Generic;

namespace WinUIBantastic.Handling.Tasks;

public static class TasksHandling
{
    public static void InitializeTasks()
    {
        TasksData.ToDoList.Clear();
        TasksData.InProgressList.Clear();
        TasksData.DoneList.Clear();

        if (TasksFile.DoesFileExist)
        {
            foreach (string newLine in File.ReadLines(TasksFile.FilePath))
            {
                switch (newLine[^1])
                {
                    case '2': TasksData.InProgressList.Add(newLine.Remove(newLine.Length - 1)); break;
                    case '3': TasksData.DoneList.Add(newLine.Remove(newLine.Length - 1)); break;
                    default: TasksData.ToDoList.Add(newLine.Remove(newLine.Length - 1)); break;
                }
            }
        }
    }
    public static async Task AddTask(string task, char listNumber)
    {
        switch (listNumber)
        {
            case '2':
                await TasksFile.WriteToFile($"{task}2");
                TasksData.InProgressList.Add($"{task}");
                break;
            case '3':
                await TasksFile.WriteToFile($"{task}3");
                TasksData.DoneList.Add($"{task}");
                break;
            default:
                await TasksFile.WriteToFile($"{task}1");
                TasksData.ToDoList.Add($"{task}");
                break;
        }
    }
    public static async Task MoveTask(ICollection<object> listOfTasks, char listNumber)
    {
        switch (listNumber)
        {
            case '1':
                foreach (object task in listOfTasks)
                {
                    if (TasksData.ToDoList.Contains(task.ToString()))
                    {
                        listOfTasks.Remove(task);
                        break;
                    }
                    else if (TasksData.InProgressList.Contains(task.ToString()))
                    {
                        TasksData.InProgressList.Remove(task.ToString());
                    }
                    else if (TasksData.DoneList.Contains(task.ToString()))
                    {
                        TasksData.DoneList.Remove(task.ToString());
                    }
                    TasksData.ToDoList.Add(task.ToString());
                }
                break;
            case '2':
                foreach (object task in listOfTasks)
                {
                    if (TasksData.InProgressList.Contains(task.ToString()))
                    {
                        listOfTasks.Remove(task);
                        break;
                    }
                    else if (TasksData.ToDoList.Contains(task.ToString()))
                    {
                        TasksData.ToDoList.Remove(task.ToString());
                    }
                    else if (TasksData.DoneList.Contains(task.ToString()))
                    {
                        TasksData.DoneList.Remove(task.ToString());
                    }
                    TasksData.InProgressList.Add(task.ToString());
                }
                break;
            default:
                foreach (object task in listOfTasks)
                {
                    if (TasksData.DoneList.Contains(task.ToString()))
                    {
                        listOfTasks.Remove(task);
                        break;
                    }
                    else if (TasksData.InProgressList.Contains(task.ToString()))
                    {
                        TasksData.InProgressList.Remove(task.ToString());
                    }
                    else if (TasksData.ToDoList.Contains(task.ToString()))
                    {
                        TasksData.ToDoList.Remove(task.ToString());
                    }
                    TasksData.DoneList.Add(task.ToString());
                }
                break;
        }
        TasksFile.DeleteFile();
        await FetchTasksFromLists();
    }
    public static async Task EraseTask(ICollection<object> listOfTasks)
    {
        TasksFile.DeleteFile();

        foreach (object task in listOfTasks)
        {
            if (TasksData.ToDoList.Contains(task.ToString()))
            {
                TasksData.ToDoList.Remove(task.ToString());
            }
            else if (TasksData.InProgressList.Contains(task.ToString()))
            {
                TasksData.InProgressList.Remove(task.ToString());
            }
            else if (TasksData.DoneList.Contains(task.ToString()))
            {
                TasksData.DoneList.Remove(task.ToString());
            }
        }

        if (!(TasksData.ToDoList.Count == 0 && TasksData.InProgressList.Count == 0 && TasksData.DoneList.Count == 0))
        {
            await FetchTasksFromLists();
        }
    }

    public static async Task RefreshToDoList() { if (TasksFile.DoesFileExist) await FetchToDoTasksFromFile(); }
    public static async Task RefreshInProgressList() { if (TasksFile.DoesFileExist) await FetchInProgressTasksFromFile(); }
    public static async Task RefreshDoneList() { if (TasksFile.DoesFileExist) await FetchDoneTasksFromFile(); }
    public static async Task RefreshAllLists() { if (TasksFile.DoesFileExist) await FetchTasksFromFile(); }

    private static async Task FetchToDoTasksFromFile()
    {
        TasksData.ToDoList.Clear();
        using StreamReader streamReader = new(TasksFile.FilePath);
        string newLine;

        while ((newLine = await streamReader.ReadLineAsync()) is not null)
        {
            if (newLine[^1] is '1')
            {
                TasksData.ToDoList.Add(newLine.Remove(newLine.Length - 1));
            }
        }

    }
    private static async Task FetchInProgressTasksFromFile()
    {
        TasksData.InProgressList.Clear();
        using StreamReader streamReader = new(TasksFile.FilePath);
        string newLine;

        while ((newLine = await streamReader.ReadLineAsync()) is not null)
        {
            if (newLine[^1] is '2')
            {
                TasksData.InProgressList.Add(newLine.Remove(newLine.Length - 1));
            }
        }
    }
    private static async Task FetchDoneTasksFromFile()
    {
        TasksData.DoneList.Clear();
        using StreamReader streamReader = new(TasksFile.FilePath);
        string newLine;

        while ((newLine = await streamReader.ReadLineAsync()) is not null)
        {
            if (newLine[^1] is '3')
            {
                TasksData.DoneList.Add(newLine.Remove(newLine.Length - 1));
            }
        }
    }
    private static async Task FetchTasksFromLists()
    {
        foreach (string line in TasksData.ToDoList)
        {
            await TasksFile.WriteToFile($"{line}1");
        }

        foreach (string line in TasksData.InProgressList)
        {
            await TasksFile.WriteToFile($"{line}2");
        }

        foreach (string line in TasksData.DoneList)
        {
            await TasksFile.WriteToFile($"{line}3");
        }
    }
    private static async Task FetchTasksFromFile()
    {
        TasksData.ToDoList.Clear();
        TasksData.InProgressList.Clear();
        TasksData.DoneList.Clear();
        using StreamReader streamReader = new(TasksFile.FilePath);
        string newLine;
        while ((newLine = await streamReader.ReadLineAsync()) is not null)
        {
            switch (newLine[^1])
            {
                case '2': TasksData.InProgressList.Add(newLine.Remove(newLine.Length - 1)); break;
                case '3': TasksData.DoneList.Add(newLine.Remove(newLine.Length - 1)); break;
                default: TasksData.ToDoList.Add(newLine.Remove(newLine.Length - 1)); break;
            }
        }
    }
}