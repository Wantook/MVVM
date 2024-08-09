using Microsoft.Maui.Controls;
using MVVM.ViewModel;

namespace MVVM.View
{
    public partial class DeleteTasksPage : ContentPage
    {
        public DeleteTasksPage()
        {
            InitializeComponent();
            WidthRequest = 300;
            HeightRequest = 500;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if (BindingContext is DeleteTasksViewModel viewModel)
            {
                viewModel.LoadTasks();
            }
            else
            {
                
                System.Diagnostics.Debug.WriteLine("BindingContext is not of type DeleteTasksViewModel");
            }
        }
    }
}
