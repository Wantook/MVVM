using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using MVVM.View;
using System.Diagnostics;

namespace MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private readonly string FilePath;

        public MainViewModel()
        {
            FilePath = Path.Combine(FileSystem.Current.AppDataDirectory, "tasks.txt");

            AddTaskCommand = new RelayCommand(AddTask);
            DeleteTaskCommand = new RelayCommand<TaskModel>(DeleteTask);
            LoadTasksCommand = new RelayCommand(LoadTasks);
            ViewAllTasksCommand = new AsyncRelayCommand(NavigateToViewAllTasks);
            DeleteTasksCommand = new AsyncRelayCommand(NavigateToDeleteTasks);

            LoadTasks();
        }

        private string _newTask;
        public string NewTask
        {
            get => _newTask;
            set => SetProperty(ref _newTask, value);
        }

        public ObservableCollection<TaskModel> Tasks { get; set; } = new ObservableCollection<TaskModel>();

        public IRelayCommand AddTaskCommand { get; }
        public IRelayCommand<TaskModel> DeleteTaskCommand { get; }
        public IRelayCommand LoadTasksCommand { get; }
        public IAsyncRelayCommand ViewAllTasksCommand { get; }
        public IAsyncRelayCommand DeleteTasksCommand { get; }

        private void AddTask()
        {
            if (!string.IsNullOrWhiteSpace(NewTask))
            {
                var task = new TaskModel { TaskName = NewTask };
                Tasks.Add(task);
                SaveTasks();
                NewTask = string.Empty;
            }
        }

        private void DeleteTask(TaskModel task)
        {
            if (task != null)
            {
                Tasks.Remove(task);
                SaveTasks();
            }
        }

        public void LoadTasks()
        {
            Tasks.Clear();
            if (File.Exists(FilePath))
            {
                var taskLines = File.ReadAllLines(FilePath);
                foreach (var line in taskLines)
                {
                    Tasks.Add(new TaskModel { TaskName = line });
                }
            }
        }

        private void SaveTasks()
        {
            var taskLines = Tasks.Select(t => t.TaskName).ToArray();
            File.WriteAllLines(FilePath, taskLines);
        }

        private async Task NavigateToViewAllTasks()
        {
            try
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync(nameof(ViewAllTasksPage));
                }
                else
                {
                    Debug.WriteLine("Shell.Current is null.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to ViewAllTasksPage: {ex.Message}");
            }
        }

        private async Task NavigateToDeleteTasks()
        {
            try
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync(nameof(DeleteTasksPage));
                }
                else
                {
                    Debug.WriteLine("Shell.Current is null.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to DeleteTasksPage: {ex.Message}");
            }
        }

    }
}
