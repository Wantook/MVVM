using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MVVM.ViewModel
{
    public class AddTaskViewModel : ObservableObject
    {
        private readonly string FilePath;
        private readonly string DescriptionsFilePath;

        public AddTaskViewModel()
        {
            FilePath = Path.Combine(FileSystem.Current.AppDataDirectory, "tasks.txt");
            DescriptionsFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, "task_descriptions.txt");
            SaveTaskCommand = new AsyncRelayCommand(SaveTask);
        }

        private string _taskName;
        public string TaskName
        {
            get => _taskName;
            set => SetProperty(ref _taskName, value);
        }

        private string _taskDescription;
        public string TaskDescription
        {
            get => _taskDescription;
            set => SetProperty(ref _taskDescription, value);
        }

        public IAsyncRelayCommand SaveTaskCommand { get; }

        private async Task SaveTask()
        {
            if (!string.IsNullOrWhiteSpace(TaskName))
            {
                
                var tasks = File.Exists(FilePath) ? File.ReadAllLines(FilePath).ToList() : new List<string>();
                tasks.Add(TaskName);
                File.WriteAllLines(FilePath, tasks);

                
                var descriptions = File.Exists(DescriptionsFilePath) ? File.ReadAllLines(DescriptionsFilePath).ToList() : new List<string>();
                descriptions.Add($"{TaskName}:{TaskDescription}");
                File.WriteAllLines(DescriptionsFilePath, descriptions);

                
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("..");
                }
            }
        }
    }
}
