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
        // Сортировка пузырьком
        public static (int[], double) BubbleSort(int[] array, bool isAscendingSortChecked)
        {
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < array.Length; ++i)
            {
                for (var j = 0; j < array.Length - 1; ++j)
                {
                    if (array[j] > array[j + 1] && isAscendingSortChecked)
                    {
                        (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    }
                    else
                    {
                        if (array[j] < array[j + 1] && !isAscendingSortChecked)
                        {
                            (array[j], array[j + 1]) = (array[j + 1], array[j]);
                        }
                    }
                }
            }
            
            stopwatch.Stop();
            return (array, new TimeSpan(stopwatch.ElapsedTicks).TotalMilliseconds);
        }
        
        // Сортировка вставками
        public static (int[], int, double) InsertionSort(int[] array, bool isAscendingSortChecked)
        {
            var stopwatch = Stopwatch.StartNew();
            var iterations = 0;
            for (var i = 1; i < array.Length; ++i)
            {
                var current = array[i];
                var j = i - 1;
                while (j >= 0 && (isAscendingSortChecked ? array[j] > current : array[j] < current))
                {
                    ++iterations;
                    array[j + 1] = array[j];
                    --j;
                }
                array[j + 1] = current;
            }
            
            stopwatch.Stop();
            return (array, iterations, new TimeSpan(stopwatch.ElapsedTicks).TotalMilliseconds);
        }
        
        // Быстрая сортировка
        public static (int[], int, double) QuickSort(int[] array, bool isAscendingSortChecked)
        {
            var stopwatch = Stopwatch.StartNew();
            var iterations = 0;
            QuickSorting(array, 0, array.Length - 1, isAscendingSortChecked, ref iterations);
            
            stopwatch.Stop();
            return (array, iterations, new TimeSpan(stopwatch.ElapsedTicks).TotalMilliseconds);
        }
        
        // Метод быстрой сортировки
        private static void QuickSorting(int[] array, int left, int right, bool isAscendingSortChecked, ref int iterations)
        {
            if (left < right)
            {
                var pivot = Partition(array, left, right, isAscendingSortChecked, ref iterations);
                QuickSorting(array, left, pivot - 1, isAscendingSortChecked, ref iterations);
                QuickSorting(array, pivot + 1, right, isAscendingSortChecked, ref iterations);
            }
        }
        
        // Метод разделения массива
        private static int Partition(int[] array, int left, int right, bool isAscendingSortChecked, ref int iterations)
        {
            var pivot = array[left];
            while (true)
            {
                while (isAscendingSortChecked ? array[left] < pivot : array[left] > pivot)
                {
                    ++left;
                    ++iterations;
                }
                while (isAscendingSortChecked ? array[right] > pivot : array[right] < pivot)
                {
                    --right;
                    ++iterations;
                }
                if (left >= right)
                {
                    return right;
                }
                (array[left], array[right]) = (array[right], array[left]);
                ++iterations;
                ++left;
                --right;
            }
        }
        
        // Шейкерная сортировка
        public static (int[], int, double) ShakerSort(int[] array, bool isAscendingSortChecked)
        {
            var stopwatch = Stopwatch.StartNew();
            var iterations = 0;
            bool swapped;
            int start = 0;
            int end = array.Length - 1;

            do
            {
                swapped = false;

                // Проход слева направо
                for (int i = start; i < end; ++i)
                {
                    if ((array[i] > array[i + 1] && isAscendingSortChecked) || 
                        (array[i] < array[i + 1] && !isAscendingSortChecked))
                    {
                        (array[i], array[i + 1]) = (array[i + 1], array[i]);
                        swapped = true;
                        ++iterations;
                    }
                }
                --end;

                // Проход справа налево
                for (int i = end; i > start; --i)
                {
                    if ((array[i - 1] > array[i] && isAscendingSortChecked) || 
                        (array[i - 1] < array[i] && !isAscendingSortChecked))
                    {
                        (array[i - 1], array[i]) = (array[i], array[i - 1]);
                        swapped = true;
                        ++iterations;
                    }
                }
                ++start;
            }
            while (swapped);

            stopwatch.Stop();
            return (array, iterations, new TimeSpan(stopwatch.ElapsedTicks).TotalMilliseconds);
        }


        // Bogo-сортировка
        public static (int[], int, double) BogoSort(int[] array, bool isAscendingSortChecked)
        {
            var stopwatch = Stopwatch.StartNew();
            var iterations = 0;
            while (!IsSorted(array, isAscendingSortChecked))
            {
                ++iterations;
                Shuffle(array);
                if (iterations > 1000)
                {
                    break;
                }
            }
            
            stopwatch.Stop();
            return (array, iterations, new TimeSpan(stopwatch.ElapsedTicks).TotalMilliseconds);
        }
        
        // Метод проверки отсортированности массива
        private static bool IsSorted(int[] array, bool isAscendingSortChecked)
        {
            for (var i = 0; i < array.Length - 1; ++i)
            {
                if (isAscendingSortChecked ? array[i] > array[i + 1] : array[i] < array[i + 1])
                {
                    return false;
                }
            }

            return true;
        }
        
        // Метод перемешивания массива
        private static void Shuffle(int[] array)
        {
            var random = new Random();
            for (var i = 0; i < array.Length; ++i)
            {
                var j = random.Next(i, array.Length);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
    }

        public class SortingViewModel : INotifyPropertyChanged
    {
        // чек-боксы выбора методов сортировок
        private bool _isBubbleSortingChecked;
        private bool _isInsertionSortingChecked;
        private bool _isQuickSortingChecked;
        private bool _isShakerSortingChecked;
        private bool _isBogoSortingChecked;
        
        public bool IsBubbleSortingChecked
        {
            get => _isBubbleSortingChecked;
            set
            {
                _isBubbleSortingChecked = value;
                OnPropertyChanged(nameof(IsBubbleSortingChecked));
            }
        }
        
        public bool IsInsertionSortingChecked
        {
            get => _isInsertionSortingChecked;
            set
            {
                _isInsertionSortingChecked = value;
                OnPropertyChanged(nameof(IsInsertionSortingChecked));
            }
        }
        
        public bool IsQuickSortingChecked
        {
            get => _isQuickSortingChecked;
            set
            {
                _isQuickSortingChecked = value;
                OnPropertyChanged(nameof(IsQuickSortingChecked));
            }
        }

        public bool IsShakerSortingChecked
        {
            get => _isShakerSortingChecked;
            set
            {
                _isShakerSortingChecked = value;
                OnPropertyChanged(nameof(IsShakerSortingChecked));
            }
        }

        public bool IsBogoSortingChecked
        {
            get => _isBogoSortingChecked;
            set
            {
                _isBogoSortingChecked = value;
                OnPropertyChanged(nameof(IsBogoSortingChecked));
            }
        }

        // среднее количество итераций сортировки
        private int _averageCountOfBubbleSortingIterations;
        private int _averageCountOfInsertionSortingIterations;
        private int _averageCountOfQuickSortingIterations;
        private int _averageCountOfShakerSortingIterations;
        private int _averageCountOfBogoSortingIterations;
        
        public int AverageCountOfBubbleSortingIterations
        {
            get => _averageCountOfBubbleSortingIterations;
            set
            {
                _averageCountOfBubbleSortingIterations = value;
                OnPropertyChanged(nameof(AverageCountOfBubbleSortingIterations));
            }
        }
        
        public int AverageCountOfInsertionSortingIterations
        {
            get => _averageCountOfInsertionSortingIterations;
            set
            {
                _averageCountOfInsertionSortingIterations = value;
                OnPropertyChanged(nameof(AverageCountOfInsertionSortingIterations));
            }
        }
        
        public int AverageCountOfQuickSortingIterations
        {
            get => _averageCountOfQuickSortingIterations;
            set
            {
                _averageCountOfQuickSortingIterations = value;
                OnPropertyChanged(nameof(AverageCountOfQuickSortingIterations));
            }
        }

        public int AverageCountOfShakerSortingIterations
        {
            get => _averageCountOfShakerSortingIterations;
            set
            {
                _averageCountOfShakerSortingIterations = value;
                OnPropertyChanged(nameof(AverageCountOfShakerSortingIterations));
            }
        }
        
        public int AverageCountOfBogoSortingIterations
        {
            get => _averageCountOfBogoSortingIterations;
            set
            {
                _averageCountOfBogoSortingIterations = value;
                OnPropertyChanged(nameof(AverageCountOfBogoSortingIterations));
            }
        }

        // среднее время выполнения сортировки
        private double _averageTimeOfBubbleSortingExecution;
        private double _averageTimeOfInsertionSortingExecution;
        private double _averageTimeOfQuickSortingExecution;
        private double _averageTimeOfShakerSortingExecution;
        private double _averageTimeOfBogoSortingExecution;
        
        public double AverageTimeOfBubbleSortingExecution
        {
            get => _averageTimeOfBubbleSortingExecution;
            set
            {
                _averageTimeOfBubbleSortingExecution = value;
                OnPropertyChanged(nameof(AverageTimeOfBubbleSortingExecution));
            }
        }
        
        public double AverageTimeOfInsertionSortingExecution
        {
            get => _averageTimeOfInsertionSortingExecution;
            set
            {
                _averageTimeOfInsertionSortingExecution = value;
                OnPropertyChanged(nameof(AverageTimeOfInsertionSortingExecution));
            }
        }
        
        public double AverageTimeOfQuickSortingExecution
        {
            get => _averageTimeOfQuickSortingExecution;
            set
            {
                _averageTimeOfQuickSortingExecution = value;
                OnPropertyChanged(nameof(AverageTimeOfQuickSortingExecution));
            }
        }

        public double AverageTimeOfShakerSortingExecution
        {
            get => _averageTimeOfShakerSortingExecution;
            set
            {
                _averageTimeOfShakerSortingExecution = value;
                OnPropertyChanged(nameof(AverageTimeOfShakerSortingExecution));
            }
        }

        public double AverageTimeOfBogoSortingExecution
        {
            get => _averageTimeOfBogoSortingExecution;
            set
            {
                _averageTimeOfBogoSortingExecution = value;
                OnPropertyChanged(nameof(AverageTimeOfBogoSortingExecution));
            }
        }

        // текст результата сортировки
        private string _resultOfBubbleSorting;
        private string _resultOfInsertionSorting;
        private string _resultOfQuickSorting;
        private string _resultOfShakerSorting;
        private string _resultOfBogoSorting;

        public string ResultOfBubbleSorting
        {
            get => _resultOfBubbleSorting;   
            set
            {
                _resultOfBubbleSorting = value;
                OnPropertyChanged(nameof(ResultOfBubbleSorting));
            }
        }
        
        public string ResultOfInsertionSorting
        {
            get => _resultOfInsertionSorting;
            set
            {
                _resultOfInsertionSorting = value;
                OnPropertyChanged(nameof(ResultOfInsertionSorting));
            }
        }
        
        public string ResultOfQuickSorting
        {
            get => _resultOfQuickSorting;
            set
            {
                _resultOfQuickSorting = value;
                OnPropertyChanged(nameof(ResultOfQuickSorting));
            }
        }

        public string ResultOfShakerSorting
        {
            get => _resultOfShakerSorting;
            set
            {
                _resultOfShakerSorting = value;
                OnPropertyChanged(nameof(ResultOfShakerSorting));
            }
        }
        
        public string ResultOfBogoSorting
        {
            get => _resultOfBogoSorting;
            set
            {
                _resultOfBogoSorting = value;
                OnPropertyChanged(nameof(ResultOfBogoSorting));
            }
        }
        
        // выбор порядка сортировки
        private bool _isAscendingSortChecked = true;
        public bool IsAscendingSortChecked
        {
            get => _isAscendingSortChecked;
            set
            {
                _isAscendingSortChecked = value;
                OnPropertyChanged(nameof(IsAscendingSortChecked));
            }
        }

        // пользовательский ввод
        private string _userInput;
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
        private int _countOfTests = 1;
        public int CountOfTests
        {
            get => _countOfTests;
            set
            {
                if (value > 0)
                {
                    _countOfTests = value;
                    OnPropertyChanged(nameof(CountOfTests));   
                }
            }
        }
        
        // длина генерируемого массива
        private int _arrayLength;
        public int ArrayLength
        {
            get => _arrayLength;
            set
            {
                _arrayLength = value;
                OnPropertyChanged(nameof(ArrayLength));
            }
        }
        
        // минимальное значение генерируемого массива
        private int _minValue;
        public int MinValue
        {
            get => _minValue;
            set
            {
                _minValue = value;
                OnPropertyChanged(nameof(MinValue));
            }
        }
        
        // максимальное значение генерируемого массива
        private int _maxValue;
        public int MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                OnPropertyChanged(nameof(MaxValue));
            }
        }
        
        public void Generate()
        {
            var inputArrayLengthWindow = new InputArrayLengthWindow();

            if (inputArrayLengthWindow.ShowDialog() == true)
            {
                int length = inputArrayLengthWindow.ArrayLength ?? 100;
                int minValue = inputArrayLengthWindow.MinValue ?? 0;
                int maxValue = inputArrayLengthWindow.MaxValue ?? 100;
            
                var numbers = new int[length];
                var rnd = new Random();
                for (var i = 0; i < length; ++i)
                {
                    numbers[i] = rnd.Next(minValue, maxValue);
                }
            
                UserInput = string.Join(" ", numbers);
            }
        }

        private void Sort()
        {
            ClearData();
            
            if (IsBubbleSortingChecked)
            {
                double sumOfExecutionTime = 0;
                
                for (var iterationOfSorting = 0; iterationOfSorting < CountOfTests; ++iterationOfSorting)
                {
                    var numbers = UserInput.Split(' ').Select(int.Parse).ToArray();
                    var result = SortingModel.BubbleSort(UserInput.Split(' ').Select(int.Parse).ToArray(), IsAscendingSortChecked);
                    ResultOfBubbleSorting = string.Join(" ", result.Item1);
                    sumOfExecutionTime += result.Item2;
                }
                
                AverageCountOfBubbleSortingIterations = UserInput.Split(' ').Select(int.Parse).ToArray().Length * (UserInput.Split(' ').Select(int.Parse).ToArray().Length - 1);
                AverageTimeOfBubbleSortingExecution = sumOfExecutionTime / CountOfTests;
            }
            
            if (IsInsertionSortingChecked)
            {
                var sumOfIterations = 0;
                double sumOfExecutionTime = 0;
                
                for (var iterationOfSorting = 0; iterationOfSorting < CountOfTests; ++iterationOfSorting)
                {
                    var numbers = UserInput.Split(' ').Select(int.Parse).ToArray();
                    var result = SortingModel.InsertionSort(numbers, IsAscendingSortChecked);
                    ResultOfInsertionSorting = string.Join(" ", result.Item1);
                    sumOfIterations += result.Item2;
                    sumOfExecutionTime += result.Item3;
                }

                AverageCountOfInsertionSortingIterations = sumOfIterations / CountOfTests;
                AverageTimeOfInsertionSortingExecution = sumOfExecutionTime / CountOfTests;
            }
            
            if (IsQuickSortingChecked)
            {
                var sumOfIterations = 0;
                double sumOfExecutionTime = 0;
                
                for (var iterationOfSorting = 0; iterationOfSorting < CountOfTests; ++iterationOfSorting)
                {
                    var numbers = UserInput.Split(' ').Select(int.Parse).ToArray();
                    var result = SortingModel.QuickSort(numbers, IsAscendingSortChecked);
                    ResultOfQuickSorting = string.Join(" ", result.Item1);
                    sumOfIterations += result.Item2;
                    sumOfExecutionTime += result.Item3;
                }

                AverageCountOfQuickSortingIterations = sumOfIterations / CountOfTests;
                AverageTimeOfQuickSortingExecution = sumOfExecutionTime / CountOfTests;
            }

            if (IsShakerSortingChecked)
            {
                var sumOfIterations = 0;
                double sumOfExecutionTime = 0;
                
                for (var iterationOfSorting = 0; iterationOfSorting < CountOfTests; ++iterationOfSorting)
                {
                    var numbers = UserInput.Split(' ').Select(int.Parse).ToArray();
                    var result = SortingModel.ShakerSort(numbers, IsAscendingSortChecked);
                    ResultOfShakerSorting = string.Join(" ", result.Item1);
                    sumOfIterations += result.Item2;
                    sumOfExecutionTime += result.Item3;
                }

                AverageCountOfShakerSortingIterations = sumOfIterations / CountOfTests;
                AverageTimeOfShakerSortingExecution = sumOfExecutionTime / CountOfTests;
            }
            
            if (IsBogoSortingChecked)
            {
                var sumOfIterations = 0;
                double sumOfExecutionTime = 0;
                
                for (var iterationOfSorting = 0; iterationOfSorting < CountOfTests; ++iterationOfSorting)
                {
                    var numbers = UserInput.Split(' ').Select(int.Parse).ToArray();
                    var result = SortingModel.BogoSort(numbers, IsAscendingSortChecked);
                    ResultOfBogoSorting = string.Join(" ", result.Item1);
                    sumOfIterations += result.Item2;
                    sumOfExecutionTime += result.Item3;
                }
                
                AverageCountOfBogoSortingIterations = sumOfIterations / CountOfTests;
                AverageTimeOfBogoSortingExecution = sumOfExecutionTime / CountOfTests;
            }
        }

        private void ClearData()
        {
            ResultOfBubbleSorting = "";
            ResultOfInsertionSorting = "";
            ResultOfQuickSorting = "";
            ResultOfShakerSorting = "";
            ResultOfBogoSorting = "";
            
            AverageCountOfBubbleSortingIterations = 0;
            AverageCountOfInsertionSortingIterations = 0;
            AverageCountOfQuickSortingIterations = 0;
            AverageCountOfShakerSortingIterations = 0;
            AverageCountOfBogoSortingIterations = 0;
            
            AverageTimeOfBubbleSortingExecution = 0;
            AverageTimeOfInsertionSortingExecution = 0;
            AverageTimeOfQuickSortingExecution = 0;
            AverageTimeOfShakerSortingExecution = 0;
            AverageTimeOfBogoSortingExecution = 0;
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