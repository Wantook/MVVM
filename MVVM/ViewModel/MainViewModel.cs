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
        private readonly string DescriptionsFilePath;

        [Obsolete]
        public MainViewModel()
        {
            FilePath = Path.Combine(FileSystem.Current.AppDataDirectory, "tasks.txt");
            DescriptionsFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, "task_descriptions.txt");

            AddTaskCommand = new AsyncRelayCommand(NavigateToAddTaskPage);
            DeleteTaskCommand = new RelayCommand<TaskModel>(DeleteTask);
            LoadTasksCommand = new RelayCommand(LoadTasks);
            ViewAllTasksCommand = new AsyncRelayCommand(NavigateToViewAllTasks);
            DeleteTasksCommand = new AsyncRelayCommand(NavigateToDeleteTasks);
            NavigateToNoteDetailsCommand = new AsyncRelayCommand<TaskModel>(NavigateToNoteDetails);

            LoadTasks();
        }

        private string _newTask;
        public string NewTask
        {
            get => _newTask;
            set => SetProperty(ref _newTask, value);
        }

        public ObservableCollection<TaskModel> Tasks { get; set; } = new ObservableCollection<TaskModel>();

        public TaskModel SelectedTask { get; set; }

        public IAsyncRelayCommand AddTaskCommand { get; }
        public IRelayCommand<TaskModel> DeleteTaskCommand { get; }
        public IRelayCommand LoadTasksCommand { get; }
        public IAsyncRelayCommand ViewAllTasksCommand { get; }
        public IAsyncRelayCommand DeleteTasksCommand { get; }
        public IAsyncRelayCommand<TaskModel> NavigateToNoteDetailsCommand { get; }

        private async Task NavigateToAddTaskPage()
        {
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync(nameof(AddTaskPage));
            }
        }

        [Obsolete]
        private void DeleteTask(TaskModel task)
        {
            if (task != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Tasks.Remove(task);
                });

                SaveTasks();

                var descriptions = File.Exists(DescriptionsFilePath) ? File.ReadAllLines(DescriptionsFilePath).ToList() : new List<string>();
                descriptions.RemoveAll(d => d.StartsWith($"{task.TaskName}:"));
                File.WriteAllLines(DescriptionsFilePath, descriptions);

                LoadTasks();
            }
        }

        [Obsolete]
        public void LoadTasks()
        {
            Device.BeginInvokeOnMainThread(() =>
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
            });
        }

        private void SaveTasks()
        {
            var taskLines = Tasks.Select(t => t.TaskName).ToArray();
            File.WriteAllLines(FilePath, taskLines);
        }

        public async Task NavigateToNoteDetails(TaskModel task)
        {
            try
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync($"{nameof(NoteDetailsPage)}?TaskName={task.TaskName}");
                }
                else
                {
                    Debug.WriteLine("Shell.Current is null.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to NoteDetailsPage: {ex.Message}");
            }
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
