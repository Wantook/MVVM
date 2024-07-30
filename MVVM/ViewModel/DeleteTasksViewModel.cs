using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace MVVM.ViewModel
{
    public class DeleteTasksViewModel : ObservableObject
    {
        private readonly string FilePath;
        private TaskModel _selectedTask;

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

        private void DeleteTask()
        {
            if (SelectedTask != null)
            {
                Tasks.Remove(SelectedTask);
                SaveTasks();
                SelectedTask = null; 
            }
        }

        private void SaveTasks()
        {
            var taskLines = Tasks.Select(t => t.TaskName).ToArray();
            File.WriteAllLines(FilePath, taskLines);
        }
    }
}
