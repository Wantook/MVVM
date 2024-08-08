using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.Maui.Controls;

namespace MVVM.ViewModel
{
    public class DeleteTasksViewModel : ObservableObject
    {
        private readonly string FilePath;
        private TaskModel _selectedTask;

        [Obsolete]
        public DeleteTasksViewModel()
        {
            FilePath = Path.Combine(FileSystem.Current.AppDataDirectory, "tasks.txt");
            LoadTasksCommand = new RelayCommand(LoadTasks);
            DeleteTaskCommand = new RelayCommand(DeleteTask);

            LoadTasks();
        }

        public ObservableCollection<TaskModel> Tasks { get; set; } = new ObservableCollection<TaskModel>();

        public TaskModel SelectedTask
        {
            get => _selectedTask;
            set => SetProperty(ref _selectedTask, value);
        }

        public IRelayCommand LoadTasksCommand { get; }
        public IRelayCommand DeleteTaskCommand { get; }

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

        [Obsolete]
        private void DeleteTask()
        {
            if (SelectedTask != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Tasks.Remove(SelectedTask);
                });

                SaveTasks();
                SelectedTask = null;

                var mainViewModel = Application.Current.MainPage.BindingContext as MainViewModel;
                mainViewModel?.LoadTasks();
            }
        }

        private void SaveTasks()
        {
            var taskLines = Tasks.Select(t => t.TaskName).ToArray();
            File.WriteAllLines(FilePath, taskLines);
        }
    }
}
