using Sorting.ViewModels;

namespace Sorting
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new SortingViewModel();
        }
    }
}