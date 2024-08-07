using MVVM.Model;
using MVVM.ViewModel;

namespace MVVM.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is TaskModel selectedTask)
            {
                var viewModel = BindingContext as MainViewModel;
                if (viewModel != null)
                {
                    await viewModel.NavigateToNoteDetails(selectedTask);
                }
            }
        }
    }
}
