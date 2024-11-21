using System.Windows;

namespace Sorting
{
    public partial class InputArrayLengthWindow : Window
    {
        public int? ArrayLength { get; private set; }
        
        public InputArrayLengthWindow()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ArrayLengthTextBox.Text, out int length) && length > 0)
            {
                ArrayLength = length;
                DialogResult = true; // Устанавливаем результат, чтобы закрыть окно
            }
            else
            {
                MessageBox.Show("Введите корректное положительное число!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}