using System.Windows;

namespace Sorting
{
    public partial class InputArrayLengthWindow : Window
    {
        public InputArrayLengthWindow()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}