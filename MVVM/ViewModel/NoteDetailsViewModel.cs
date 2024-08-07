using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace MVVM.ViewModel
{
    public class NoteDetailsViewModel : ObservableObject
    {
        private readonly string FilePath;
        private readonly string DescriptionsFilePath;

        public NoteDetailsViewModel()
        {
            FilePath = Path.Combine(FileSystem.Current.AppDataDirectory, "tasks.txt");
            DescriptionsFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, "task_descriptions.txt");
            DeleteTaskCommand = new RelayCommand(DeleteTask);
            SaveDescriptionCommand = new RelayCommand(SaveDescription);
        }

        private TaskModel _task;
        public TaskModel Task
        {
            get => _task;
            set => SetProperty(ref _task, value);
        }

        private string _taskDescription;
        public string TaskDescription
        {
            get => _taskDescription;
            set => SetProperty(ref _taskDescription, value);
        }

        public IRelayCommand DeleteTaskCommand { get; }
        public IRelayCommand SaveDescriptionCommand { get; }

        public void LoadTask(string taskName)
        {
            if (File.Exists(FilePath))
            {
                var taskLines = File.ReadAllLines(FilePath);
                var task = taskLines.FirstOrDefault(t => t == taskName);
                if (task != null)
                {
                    Task = new TaskModel { TaskName = task };
                    LoadDescription(taskName);
                }
            }
        }

        private void LoadDescription(string taskName)
        {
            if (File.Exists(DescriptionsFilePath))
            {
                var descriptions = File.ReadAllLines(DescriptionsFilePath);
                var description = descriptions.FirstOrDefault(d => d.StartsWith($"{taskName}:"));
                if (description != null)
                {
                    TaskDescription = description.Substring(taskName.Length + 1);
                }
            }
        }

        private void DeleteTask()
        {
            if (Task != null)
            {
                var tasks = File.ReadAllLines(FilePath).ToList();
                tasks.Remove(Task.TaskName);
                File.WriteAllLines(FilePath, tasks);

                var descriptions = File.ReadAllLines(DescriptionsFilePath).ToList();
                descriptions.RemoveAll(d => d.StartsWith($"{Task.TaskName}:"));
                File.WriteAllLines(DescriptionsFilePath, descriptions);

                Task = null;
                TaskDescription = string.Empty;
            }
        }

        private void SaveDescription()
        {
            if (Task != null)
            {
                var descriptions = File.Exists(DescriptionsFilePath)
                    ? File.ReadAllLines(DescriptionsFilePath).ToList()
                    : new List<string>();

                var existingDescription = descriptions.FirstOrDefault(d => d.StartsWith($"{Task.TaskName}:"));
                if (existingDescription != null)
                {
                    descriptions.Remove(existingDescription);
                }

                descriptions.Add($"{Task.TaskName}:{TaskDescription}");
                File.WriteAllLines(DescriptionsFilePath, descriptions);
            }
        }
    }
}
