using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Diagnostics;
using System.Globalization;
using System.Windows;


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


    public static class SortingModel
    {
        // Сортировка пузырьком
        public static (MixedValue[], int, double) BubbleSort(MixedValue[] array, bool isAscendingSortChecked)
        {
            if (IsSorted(array, isAscendingSortChecked))
            {
                return (array, 0, 0);
            }
            
            var stopwatch = Stopwatch.StartNew();
            var iterations = 0;
            
            for (var i = 0; i < array.Length - 1; i++)
            {
                for (var j = 0; j < array.Length - i - 1; j++)
                {
                    iterations++;
                    var needSwap = isAscendingSortChecked
                        ? array[j].CompareTo(array[j + 1]) > 0
                        : array[j].CompareTo(array[j + 1]) < 0;

                    if (needSwap)
                    {
                        (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    }
                }
            }

            stopwatch.Stop();
            return (array, iterations, stopwatch.Elapsed.TotalMilliseconds);
        }
        
        // Сортировка вставками
        public static (MixedValue[], int, double) InsertionSort(MixedValue[] array, bool isAscendingSortChecked)
        {
            if (IsSorted(array, isAscendingSortChecked))
            {
                return (array, 0, 0);
            }
            
            var stopwatch = Stopwatch.StartNew();
            var iterations = 0;
            
            for (var i = 1; i < array.Length; ++i)
            {
                var current = array[i];
                var j = i - 1;
                while (j >= 0 && (isAscendingSortChecked 
                           ? array[j].CompareTo(current) > 0 
                           : array[j].CompareTo(current) < 0))
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
        public static (MixedValue[], int, double) QuickSort(MixedValue[] array, bool isAscendingSortChecked)
        {
            var stopwatch = Stopwatch.StartNew();
            var iterations = 0;

            QuickSorting(array, 0, array.Length - 1, isAscendingSortChecked, ref iterations);

            stopwatch.Stop();
            return (array, iterations, new TimeSpan(stopwatch.ElapsedTicks).TotalMilliseconds);
        }

        // Метод быстрой сортировки
        private static void QuickSorting(MixedValue[] array, int left, int right, bool isAscendingSortChecked, ref int iterations)
        {
            if (left < right)
            {
                var pivot = Partition(array, left, right, isAscendingSortChecked, ref iterations);
                QuickSorting(array, left, pivot - 1, isAscendingSortChecked, ref iterations);
                QuickSorting(array, pivot + 1, right, isAscendingSortChecked, ref iterations);
            }
        }

        // Метод разделения массива
        private static int Partition(MixedValue[] array, int left, int right, bool isAscendingSortChecked, ref int iterations)
        {
            var pivot = array[left];

            while (true)
            {
                while (isAscendingSortChecked
                           ? array[left].CompareTo(pivot) < 0
                           : array[left].CompareTo(pivot) > 0)
                {
                    ++left;
                    ++iterations;
                }

                while (isAscendingSortChecked
                           ? array[right].CompareTo(pivot) > 0
                           : array[right].CompareTo(pivot) < 0)
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
        public static (MixedValue[], int, double) ShakerSort(MixedValue[] array, bool isAscendingSortChecked)
        {
            if (IsSorted(array, isAscendingSortChecked))
            {
                return (array, 0, 0);
            }
            
            var stopwatch = Stopwatch.StartNew();
            var iterations = 0;

            var left = 0;
            var right = array.Length - 1;
            while (left <= right)
            {
                // Проход справа налево
                for (var i = right; i > left; --i)
                {
                    if ((isAscendingSortChecked && array[i - 1].CompareTo(array[i]) > array[i].CompareTo(array[i - 1])) ||
                        (!isAscendingSortChecked && array[i - 1].CompareTo(array[i]) < array[i].CompareTo(array[i - 1])))
                    {
                        (array[i - 1], array[i]) = (array[i], array[i - 1]);
                        ++iterations;
                    }
                }
                ++left;

                // Проход слева направо
                for (var i = left; i < right; ++i)
                {
                    if ((isAscendingSortChecked && array[i].CompareTo(array[i + 1]) > array[i + 1].CompareTo(array[i])) ||
                        (!isAscendingSortChecked && array[i].CompareTo(array[i + 1]) < array[i + 1].CompareTo(array[i])))
                    {
                        (array[i], array[i + 1]) = (array[i + 1], array[i]);
                        ++iterations;
                    }
                }
                --right;
            }

            stopwatch.Stop();
            return (array, iterations, new TimeSpan(stopwatch.ElapsedTicks).TotalMilliseconds);
        }


        // Bogo-сортировка
        public static (MixedValue[], int, double) BogoSort(MixedValue[] array, bool isAscendingSortChecked)
        {
            if (IsSorted(array, isAscendingSortChecked))
            {
                return (array, 0, 0);
            }
            
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
        private static bool IsSorted(MixedValue[] array, bool isAscendingSortChecked)
        {
            for (var i = 0; i < array.Length - 1; ++i)
            {
                var comparisonResult = array[i].CompareTo(array[i + 1]);
                if (isAscendingSortChecked)
                {
                    // Если по возрастанию, проверяем, что текущий элемент больше следующего
                    if (comparisonResult > 0)
                    {
                        return false;
                    }
                }
                else
                {
                    // Если по убыванию, проверяем, что текущий элемент меньше следующего
                    if (comparisonResult < 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
        // Метод перемешивания массива
        private static void Shuffle(MixedValue[] array)
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
        
        // выбор типа данных сортировки
        private bool _isNumbersSortingChecked = true;
        public bool IsNumbersSortingChecked
        {
            get => _isNumbersSortingChecked;
            set
            {
                _isNumbersSortingChecked = value;
                OnPropertyChanged(nameof(IsNumbersSortingChecked));
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
        
        // Выгрузка данных из файла в текстовое поле
        private void LoadData()
        {
            // Создание диалогового окна выбора файла
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                // Filter = "Excel Files (*.xlsx)|*.xlsx|CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt",
                Filter = "All Files|(*.*)|CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt",
                Title = "Выберите файл для загрузки"
            };

            // Если пользователь выбрал файл
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var filePath = openFileDialog.FileName;
                    var fileContent = "";

                    // Чтение содержимого файла в зависимости от формата
                    if (filePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                    {
                        // fileContent = LoadExcelFile(filePath);
                    }
                    else if (filePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase) ||
                             filePath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        fileContent = System.IO.File.ReadAllText(filePath);
                    }
                    else
                    {
                        throw new NotSupportedException("Неподдерживаемый формат файла.");
                    }

                    // Обновляем текстовое поле UserInput
                    UserInput = string.Join(" ", fileContent.Split(new[] { ',', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                }
                catch (Exception ex)
                {
                    // Обработка ошибок
                    System.Windows.MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

        private void Sort()
        {
            ClearData();
            
            if (IsBubbleSortingChecked)
            {
                ExecuteSorting(
                    SortingModel.BubbleSort,
                    UserInput,
                    CountOfTests,
                    IsAscendingSortChecked,
                    out var resultString,
                    out var averageTime,
                    out var averageIterations);
                
                ResultOfBubbleSorting = resultString;
                AverageTimeOfBubbleSortingExecution = averageTime;
                AverageCountOfBubbleSortingIterations = averageIterations;
            }
            
            if (IsInsertionSortingChecked)
            {
                ExecuteSorting(
                    SortingModel.InsertionSort,
                    UserInput,
                    CountOfTests,
                    IsAscendingSortChecked,
                    out var resultString,
                    out var averageTime,
                    out var averageIterations);
                
                ResultOfInsertionSorting = resultString;
                AverageTimeOfInsertionSortingExecution = averageTime;
                AverageCountOfInsertionSortingIterations = averageIterations;
            }
            
            if (IsQuickSortingChecked)
            {
                ExecuteSorting(
                    SortingModel.QuickSort,
                    UserInput,
                    CountOfTests,
                    IsAscendingSortChecked,
                    out var resultString,
                    out var averageTime,
                    out var averageIterations);
                
                ResultOfQuickSorting = resultString;
                AverageTimeOfQuickSortingExecution = averageTime;
                AverageCountOfQuickSortingIterations = averageIterations;
            }

            if (IsShakerSortingChecked)
            {
                ExecuteSorting(
                    SortingModel.ShakerSort,
                    UserInput,
                    CountOfTests,
                    IsAscendingSortChecked,
                    out var resultString,
                    out var averageTime,
                    out var averageIterations);
                
                ResultOfShakerSorting = resultString;
                AverageTimeOfShakerSortingExecution = averageTime;
                AverageCountOfShakerSortingIterations = averageIterations;
            }
            
            if (IsBogoSortingChecked)
            {
                ExecuteSorting(
                    SortingModel.BogoSort,
                    UserInput,
                    CountOfTests,
                    IsAscendingSortChecked,
                    out var resultString,
                    out var averageTime,
                    out var averageIterations);
                
                ResultOfBogoSorting = resultString;
                AverageTimeOfBogoSortingExecution = averageTime;
                AverageCountOfBogoSortingIterations = averageIterations;
            }
        }
        
        private void ExecuteSorting(
            SortingMethod sortingMethod,
            string userInput,
            int countOfTests,
            bool isAscendingSortChecked,
            out string resultString,
            out double averageTime,
            out int averageIterations)
        {
            var sumOfIterations = 0;
            var sumOfExecutionTime = 0.0;
            var mixedArray = Array.Empty<MixedValue>();

            try
            {
                for (var iterationOfSorting = 0; iterationOfSorting < countOfTests; ++iterationOfSorting)
                {
                    mixedArray = UserInput.Split(' ')
                        .Select(value => new MixedValue(value))
                        .ToArray();

                    var result = sortingMethod(mixedArray, isAscendingSortChecked);
                    sumOfIterations += result.Item2;
                    sumOfExecutionTime += result.Item3;
                }
                
                resultString = string.Join(" ", mixedArray.Select(m => m.ToString()));;
                averageIterations = sumOfIterations / countOfTests;
                averageTime = sumOfExecutionTime / countOfTests;
            }
            catch
            {
                resultString = "Некорректный ввод";
                averageIterations = 0;
                averageTime = 0;
            }
        }
        
        private delegate (MixedValue[], int, double) SortingMethod(MixedValue[] array, bool isAscending);

        // Команды для вызова метода
        public ICommand SortCommand { get; }
        public ICommand GenerateCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand LoadDataCommand { get; }

        public SortingViewModel()
        {
            // Привязываем команды к методу
            SortCommand = new RelayCommand(_ => Sort());
            GenerateCommand = new RelayCommand(_ => Generate());
            ClearCommand = new RelayCommand(_ => ClearData());
            LoadDataCommand = new RelayCommand(_ => LoadData());
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
    
    public class MixedValue : IComparable<MixedValue>
    {
        public double? NumericValue { get; }
        public string StringValue { get; }
        public bool IsNumeric => NumericValue.HasValue;

        public MixedValue(string value)
        {
            if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
            {
                NumericValue = number;
            }
            else
            {
                StringValue = value;
            }
        }

        public int CompareTo(MixedValue other)
        {
            if (this.IsNumeric && other.IsNumeric)
            {
                return this.NumericValue.Value.CompareTo(other.NumericValue.Value);
            }
            if (!this.IsNumeric && !other.IsNumeric)
            {
                return string.Compare(this.StringValue, other.StringValue, StringComparison.Ordinal);
            }
            // Числа идут перед строками
            return this.IsNumeric ? -1 : 1;
        }

        public override string ToString()
        {
            return IsNumeric ? NumericValue.ToString() : StringValue;
        }
    }
}