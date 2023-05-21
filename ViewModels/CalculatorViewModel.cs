using System.Linq;
using System.Windows.Input;

namespace Calculadora.ViewModels
{
    public class CalculatorViewModel : BindableObject
    {
        private string currentResult = "0";
        private string currentOperation = "";
        private string mathSymbol = "";
        private double result = 0;

        public string CurrentResult
        {
            get { return currentResult; }
            set
            {
                currentResult = value;
                OnPropertyChanged();
            }
        }
        public string CurrentOperation
        {
            get { return currentOperation; }
            set
            {
                currentOperation = value;
                OnPropertyChanged();
            }
        }

        public ICommand NumberButtonCommand { get; }
        public ICommand OperatorButtonCommand { get; }
        public ICommand EqualButtonCommand { get; }
        public ICommand ClearButtonCommand { get; }
        public ICommand DeleteButtonCommand { get; }
        public ICommand PercentButtonCommand { get; }

        public CalculatorViewModel()
        {
            NumberButtonCommand = new Command<string>(OnNumberButtonClicked);
            OperatorButtonCommand = new Command<string>(OnOperatorButtonClicked);
            EqualButtonCommand = new Command(OnEqualButtonClicked);
            ClearButtonCommand = new Command(OnClearButtonClicked);
            PercentButtonCommand = new Command<string>(OnPercentButtonClicked);
            DeleteButtonCommand = new Command(OnDeleteButtonClicked);
        }
        private void OnPercentButtonClicked(string op)
        {

            if (currentOperation.Split(" ").Length < 3  && op.Equals("%"))
            {
                result = double.Parse(currentOperation) / 100;
                currentResult = result.ToString();
                CurrentResult = currentResult;
                currentOperation = "";
                CurrentOperation = "";
            }
        }

        private void OnDeleteButtonClicked()
        {
            if (currentOperation == "0")
            {
                currentOperation = "";
            }

            CurrentOperation = currentOperation.Remove(currentOperation.Length - 1);
        }

        private void OnNumberButtonClicked(string number)
        {
            if (currentOperation == "0" && number != ".")
            {
                currentOperation = "";
            }
            currentOperation += " " + number;
            CurrentOperation = currentOperation;

        }

        private void OnOperatorButtonClicked(string op)
        {
            if (!string.IsNullOrEmpty(currentOperation) && IsNumber((currentOperation.Split(" ").ToList().Last())))
            {
                mathSymbol = op;
                currentOperation += " " + mathSymbol;
                CurrentOperation = currentOperation;
            }
        }

        private void OnEqualButtonClicked()
        {
            if (!string.IsNullOrEmpty(currentOperation) && !currentOperation.Equals("%") && IsNumber((currentOperation.Split(" ").ToList().Last())) )
            {
                result = Calculate(currentOperation.Split(" ").ToList());

                currentResult = result.ToString();
                CurrentResult = currentResult;
                mathSymbol = "";
            }
        }

        private void OnClearButtonClicked()
        {
            currentResult = "";
            currentOperation = "";
            result = 0;
            CurrentResult = "0";
            CurrentOperation = "";
        }

        private static double Calculate(List<string> operation)
        {
            Stack<double> numbers = new Stack<double>();
            Stack<string> operators = new Stack<string>();
            operation.RemoveAt(0);
            foreach (string element in operation)
            {
                if (IsNumber(element))
                {
                    numbers.Push(Convert.ToDouble(element));
                }
                else
                {
                    while (operators.Count > 0 && HaveMorePriority(element, operators.Peek()))
                    {
                        double secondNumber = numbers.Pop();
                        double firstNumber = numbers.Pop();
                        string symbol = operators.Pop();
                        double operationResult = doOperation(firstNumber, symbol, secondNumber);
                        numbers.Push(operationResult);
                    }

                    operators.Push(element);
                }
            }

            while (operators.Count > 0)
            {
                double secondNumber = numbers.Pop();
                double firstNumber = numbers.Pop();
                string symbol = operators.Pop();
                double operationResult = doOperation(firstNumber, symbol, secondNumber);
                numbers.Push(operationResult);
            }

            return numbers.Pop();
        }

        private static bool IsNumber(string texto)
        {
            double number;
            return double.TryParse(texto, out number);
        }

        private static double doOperation(double number1, string symbol, double number2)
        {
            switch (symbol)
            {
                case "+":
                    return number1 + number2;
                case "-":
                    return number1 - number2;
                case "*":
                    return number1 * number2;
                case "/":
                    return number1 / number2;
                default:
                    throw new ArgumentException("Operador inválido");
            }
        }

        private static bool HaveMorePriority(string operator1, string operator2)
        {
            if ((operator2 == "*" || operator2 == "/") && (operator1 == "+" || operator1 == "-"))
            {
                return false;
            }

            return true;
        }
    }
}
