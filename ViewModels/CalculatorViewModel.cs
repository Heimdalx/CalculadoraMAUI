using System.Windows.Input;

namespace Calculadora.ViewModels
{
    public class CalculatorViewModel : BindableObject
    {
        private string currentNumber = "";
        private string operation = "";
        private double result = 0;

        public string CurrentNumber
        {
            get { return currentNumber; }
            set
            {
                currentNumber = value;
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
            if (currentNumber == "0" && number != ".")
            {
                currentNumber = "";
            }

            currentNumber += number;
            CurrentNumber = currentNumber;
        }

        private void OnOperatorButtonClicked(string op)
        {
            if (!string.IsNullOrEmpty(currentNumber))
            {
                operation = op;
                result = double.Parse(currentNumber);
                currentNumber = "";
            }
        }

        private void OnEqualButtonClicked()
        {
            if (!string.IsNullOrEmpty(currentNumber) && !string.IsNullOrEmpty(operation))
            {
                double secondNumber = double.Parse(currentNumber);
                switch (operation)
                {
                    case "+":
                        result += secondNumber;
                        break;
                    case "-":
                        result -= secondNumber;
                        break;
                    case "×":
                        result *= secondNumber;
                        break;
                    case "÷":
                        result /= secondNumber;
                        break;
                }

                currentNumber = result.ToString();
                CurrentNumber = currentNumber;
                operation = "";
            }
        }

        private void OnClearButtonClicked()
        {
            currentNumber = "";
            operation = "";
            result = 0;
            CurrentNumber = "0";
        }
    }
}
