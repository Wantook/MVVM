using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private string _newTask;
        public string NewTask
        {
            get => _newTask;
            set => SetProperty(ref _newTask, value);
        }

        public ObservableCollection<TaskModel> Tasks { get; set; } = new ObservableCollection<TaskModel>();

        public IRelayCommand AddTaskCommand { get; }

        public MainViewModel()
        {
            AddTaskCommand = new RelayCommand(AddTask);
        }

        private void AddTask()
        {
            if (!string.IsNullOrWhiteSpace(NewTask))
            {
                Tasks.Add(new TaskModel { TaskName = NewTask });
                NewTask = string.Empty;
            }
        }
    }
}
