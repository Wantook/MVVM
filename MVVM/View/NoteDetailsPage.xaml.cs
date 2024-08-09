using Microsoft.Maui.Controls;
using MVVM.ViewModel;

namespace MVVM.View
{
    [QueryProperty(nameof(TaskName), "TaskName")]
    public partial class NoteDetailsPage : ContentPage
    {
        public NoteDetailsPage()
        {
            InitializeComponent();
            WidthRequest = 300;
            HeightRequest = 500;
        }

        public string TaskName
        {
            set
            {
                if (BindingContext is NoteDetailsViewModel viewModel)
                {
                    viewModel.LoadTask(value);
                }
            }
        }
    }
}
