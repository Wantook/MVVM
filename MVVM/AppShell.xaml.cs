using MVVM.View;

namespace MVVM
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ViewAllTasksPage), typeof(ViewAllTasksPage));
            Routing.RegisterRoute(nameof(DeleteTasksPage), typeof(DeleteTasksPage));
            Routing.RegisterRoute(nameof(NoteDetailsPage), typeof(NoteDetailsPage));
            Routing.RegisterRoute(nameof(AddTaskPage), typeof(AddTaskPage));
        }
    }
}
