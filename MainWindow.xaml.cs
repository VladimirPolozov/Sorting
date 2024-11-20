using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Sorting
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new SortingViewModel();
        }
    }


    public class SortingModel
    {
        // методы сортировки
    }

    public class SortingViewModel : INotifyPropertyChanged
    {
        private bool isBubbleSortingChecked;
        private string resultOfBubbleSorting;

        public bool IsBubbleSortingChecked
        {
            get => isBubbleSortingChecked;
            set
            {
                isBubbleSortingChecked = value;
                OnPropertyChanged(nameof(IsBubbleSortingChecked));
            }
        }

        public string ResultOfBubbleSorting
        {
            get => resultOfBubbleSorting;   
            set
            {
                resultOfBubbleSorting = value;
                OnPropertyChanged(nameof(ResultOfBubbleSorting));
            }
        }

        private void Sort() {
            if (IsBubbleSortingChecked) {
                ResultOfBubbleSorting = "True";
            } else {
                ResultOfBubbleSorting = "False";
            }
        }

        // Команды для вызова метода
        public ICommand SortCommand { get; }

        public SortingViewModel()
        {
            // очистить данные
            // ClearData();

            // Привязываем команды к методу
            SortCommand = new RelayCommand(_ => Sort());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}