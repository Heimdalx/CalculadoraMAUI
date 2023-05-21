using System.Collections.Generic;
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

        public CalculatorViewModel()
        {
            NumberButtonCommand = new Command<string>(OnNumberButtonClicked);
            OperatorButtonCommand = new Command<string>(OnOperatorButtonClicked);
            EqualButtonCommand = new Command(OnEqualButtonClicked);
            ClearButtonCommand = new Command(OnClearButtonClicked);
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
            if (!string.IsNullOrEmpty(currentOperation))
            {
                mathSymbol = op;
                currentOperation += " " + mathSymbol;
                CurrentOperation = currentOperation;
            }
        }

        private void OnEqualButtonClicked()
        {
            if (!string.IsNullOrEmpty(currentOperation))
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

        public static double Calculate(List<string> operation)
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
                        double segundoNumero = numbers.Pop();
                        double primerNumero = numbers.Pop();
                        string operador = operators.Pop();
                        double resultadoOperacion = RealizarOperacion(primerNumero, operador, segundoNumero);
                        numbers.Push(resultadoOperacion);
                    }

                    operators.Push(element);
                }
            }

            while (operators.Count > 0)
            {
                double segundoNumero = numbers.Pop();
                double primerNumero = numbers.Pop();
                string operador = operators.Pop();
                double resultadoOperacion = RealizarOperacion(primerNumero, operador, segundoNumero);
                numbers.Push(resultadoOperacion);
            }

            return numbers.Pop();
        }

        public static bool IsNumber(string texto)
        {
            double numero;
            return double.TryParse(texto, out numero);
        }

        public static double RealizarOperacion(double numero1, string operador, double numero2)
        {
            switch (operador)
            {
                case "+":
                    return numero1 + numero2;
                case "-":
                    return numero1 - numero2;
                case "*":
                    return numero1 * numero2;
                case "/":
                    return numero1 / numero2;
                default:
                    throw new ArgumentException("Operador inválido");
            }
        }

        public static bool HaveMorePriority(string operador1, string operador2)
        {
            if ((operador2 == "*" || operador2 == "/") && (operador1 == "+" || operador1 == "-"))
            {
                return false;
            }

            return true;
        }
    }
}
