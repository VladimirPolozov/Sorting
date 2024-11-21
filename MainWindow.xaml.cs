using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;


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
        public static (int[], int, int) BubbleSort(int[] array, bool isAscendingSortChecked, int countOfIterations)
        {
            var sumOfIterations = 0;
            var stopwatch = Stopwatch.StartNew();
            for (var iterationOfSorting = 0; iterationOfSorting < countOfIterations; ++iterationOfSorting)
            {
                var iterations = 0;
                for (var i = 0; i < array.Length; i++)
                {
                    for (var j = 0; j < array.Length - 1; j++)
                    {
                        if (array[j] > array[j + 1] && isAscendingSortChecked)
                        {
                            ++iterations;
                            (array[j], array[j + 1]) = (array[j + 1], array[j]);
                        }
                        else
                        {
                            if (array[j] < array[j + 1] && !isAscendingSortChecked)
                            {
                                ++iterations;
                                (array[j], array[j + 1]) = (array[j + 1], array[j]);
                            }
                        }
                    }
                }
                sumOfIterations += iterations;
            }
            
            stopwatch.Stop();
            var averageTime = new TimeSpan(stopwatch.ElapsedTicks / countOfIterations);

            return (array, sumOfIterations / countOfIterations, (int)averageTime.TotalMilliseconds);
        }
    }

    public class SortingViewModel : INotifyPropertyChanged
    {
        private bool _isBubbleSortingChecked;
        
        private string _resultOfBubbleSorting;

        private int _averageCountOfBubbleSortingIterations;
        
        private int _averageTimeOfBubbleSortingExecution;

        private bool _isAscendingSortChecked = true;
        
        private string _userInput;
        private int _countOfTests;
        private int _arrayLength;
        
        // чек-боксы выбора методов сортировок
        public bool IsBubbleSortingChecked
        {
            get => _isBubbleSortingChecked;
            set
            {
                _isBubbleSortingChecked = value;
                OnPropertyChanged(nameof(IsBubbleSortingChecked));
            }
        }

        // текст результата сортировки
        public string ResultOfBubbleSorting
        {
            get => _resultOfBubbleSorting;   
            set
            {
                _resultOfBubbleSorting = value;
                OnPropertyChanged(nameof(ResultOfBubbleSorting));
            }
        }
        
        // выбор порядка сортировки
        public bool IsAscendingSortChecked
        {
            get => _isAscendingSortChecked;
            set
            {
                _isAscendingSortChecked = value;
                OnPropertyChanged(nameof(IsAscendingSortChecked));
            }
        }
        
        // среднее количество итераций сортировки
        public int AverageCountOfBubbleSortingIterations
        {
            get => _averageCountOfBubbleSortingIterations;
            set
            {
                _averageCountOfBubbleSortingIterations = value;
                OnPropertyChanged(nameof(AverageCountOfBubbleSortingIterations));
            }
        }
        
        // среднее время выполнения сортировки
        public int AverageTimeOfBubbleSortingExecution
        {
            get => _averageTimeOfBubbleSortingExecution;
            set
            {
                _averageTimeOfBubbleSortingExecution = value;
                OnPropertyChanged(nameof(AverageTimeOfBubbleSortingExecution));
            }
        }

        // пользовательский ввод
        public string UserInput
        {
            get => _userInput;
            set
            {
                _userInput = value;
                OnPropertyChanged(nameof(UserInput));
            }
        }
        
        // количество тестов
        public int CountOfTests
        {
            get => _countOfTests;
            set
            {
                _countOfTests = value;
                OnPropertyChanged(nameof(CountOfTests));
            }
        }
        
        public int ArrayLength
        {
            get => _arrayLength;
            set
            {
                _arrayLength = value;
                OnPropertyChanged(nameof(ArrayLength));
            }
        }
        
        private void Generate()
        {
            int length = ArrayLength;
            var inputArrayLengthWindow = new InputArrayLengthWindow();
            inputArrayLengthWindow.ShowDialog();
            
            var numbers = new int[length];
            var rnd = new Random();
            for (var i = 0; i < length; ++i)
            {
                numbers[i] = rnd.Next(0, 100);
            }
            
            UserInput = string.Join(" ", numbers);
        }

        private void Sort() {
            int[] numbers = UserInput.Split(' ').Select(int.Parse).ToArray();

            if (IsBubbleSortingChecked)
            {
                var result = SortingModel.BubbleSort(numbers, IsAscendingSortChecked, CountOfTests);
                ResultOfBubbleSorting = string.Join(" ", result.Item1);
                AverageCountOfBubbleSortingIterations = result.Item2;
                AverageTimeOfBubbleSortingExecution = result.Item3;
            }
        }

        // Команды для вызова метода
        public ICommand SortCommand { get; }
        public ICommand GenerateCommand { get; }

        public SortingViewModel()
        {
            // очистить данные
            // ClearData();

            // Привязываем команды к методу
            SortCommand = new RelayCommand(_ => Sort());
            GenerateCommand = new RelayCommand(_ => Generate());
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