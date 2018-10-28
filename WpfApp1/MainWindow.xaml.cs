using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        string LeftOperator = ""; // Переменная левого оператора 
        string operation = ""; // Переменная операции 
        string RightOperator = ""; // Переменная правого оператора 
        string Result = ""; // Переменная результата 
        string Equally = ""; // Переменная оператора равно 
        Regex Numbers = new Regex("[0-9]"); // Шаблон регулярного выражение (Только цифры)
        Regex Signs = new Regex("[+||*||/||-]"); // Шаблон регулрного выражения (Только знаки +, *, /, -)

        public MainWindow()
        {
            InitializeComponent();

            foreach (UIElement c in LayoutRoot.Children) // Чтение с LayoutRoot 
            {
                if (c is Button) // Если кнопка нажата, то переходим к методу Button_Click
                {
                    ((Button)c).Click += Button_Click;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) // Обработчик события нажатия на кнопки 
        {
            
            string DataButtons = (string)((Button)e.OriginalSource).Content; // Переменная который присваивается номер или знак нажатой кнопки 

            if ((!Numbers.IsMatch(DataButtons) && LeftOperator == "" && RightOperator == "") || (Result != "" && RightOperator != "" && !Numbers.IsMatch(DataButtons) && operation != "" && Equally == "=") || 
                (Result != "" && RightOperator == "" && !Numbers.IsMatch(DataButtons) && operation != "") || 
                (Result == "" && RightOperator == "" && !Numbers.IsMatch(DataButtons) && operation != "")) // Проверяется условие при которых "нельзя будет вводить" в строку калькулятора 
            {
                textBlock.Text = ""; // Очищаю textBlock
                DataButtons = ""; // Очищаю переменную s
            }
            else
            {
                textBlock.Text += DataButtons; // Присваиваю textBlock(Строке калькулятора) значение DataButton
            }


            bool result = Double.TryParse(DataButtons, out double num); // Переменная для проверки знака или цифры

            if (result == true || DataButtons == ",") // Проверка если цифра или оператор ","
            {

                if (operation == "") // Проверяем если оператор пустой, то к левому оператору добавляем переменную DataButton
                {
                    LeftOperator += DataButtons; // Присваиваем левому оператору данные нажатой кнопки 
                }
                else
                {
                    RightOperator += DataButtons; // Присваиваем правому оператору данные нажатой кнопки 
                }
            }

            else
            {

                if (DataButtons == "=" && (Numbers.IsMatch(LeftOperator) && Numbers.IsMatch(RightOperator))) // Проверяем можно ли вычислить значение если оператор равен "="
                {
                    Update_RightOperator(); // Вызваем метод вычисления результата 
                    textBlock.Text = RightOperator; // Присваиваем правый оператор (Прошедший через метод)
                    operation = ""; // Присваиваем оператору пусто 
                    Result = RightOperator; // Присваиваем результату правый оператор 
                }

                else if (Signs.IsMatch(DataButtons) && (Numbers.IsMatch(LeftOperator) && Numbers.IsMatch(RightOperator)) && operation != "") // Проверяем можно ли вычислить значение если оператор равен не пусто и DataButtons цифра
                {
                    Update_RightOperator(); // Вызваем метод вычисления результата 
                    textBlock.Text = RightOperator + DataButtons; // Складываем правый оператор и значения DataButtons
                    LeftOperator = RightOperator; // Присваиваем левому оператору правый оператор(результат)
                    Result = RightOperator; // Присваиваем результату правый оператор 
                    RightOperator = ""; // Очищаем правый оператор 

                }

                else if (DataButtons == "C") // Проверяем если нажата кнопка "C", то удалем все 
                {
                    LeftOperator = ""; // Очищаем левый оператор 
                    RightOperator = ""; // Очищаем правый оператор 
                    operation = ""; // Очищаем оператор 
                    textBlock.Text = ""; // Очищаем строку калькулятора
                }

                else
                {
                    if (Numbers.IsMatch(RightOperator) && Numbers.IsMatch(LeftOperator)) // Проверяем если правый и левый операторы цифры 
                    {
                        Update_RightOperator(); // Вызваем метод вычисления результата 
                        LeftOperator = RightOperator; // Присваиваем левому оператору правый оператор(результат)
                        RightOperator = ""; // Очищаем правый оператор 
                        Result = LeftOperator; // Присваиваем результату левый оператор 
                    }
                    if (DataButtons != "") // Прооверем если не пустая кнопка 
                    {
                        operation = DataButtons; // Присваиваем оператору значение DataButtons
                    }
                }
            }
        }

        private void Update_RightOperator() // Метод вычисления значения 
        {
            double num1 = Double.Parse(LeftOperator); // Первая переменная(Левый оператор) 
            double num2 = Double.Parse(RightOperator); // Вторая переменная(Правый оператор)

            switch (operation)
            {
                case "+":
                    RightOperator = (num1 + num2).ToString(); // Если "+", складываем
                    break;
                case "-":
                    RightOperator = (num1 - num2).ToString(); // Если "-", вычитаем 
                    break;
                case "*":
                    RightOperator = (num1 * num2).ToString(); // Если "*", умножаем 
                    break;
                case "/":
                    RightOperator = (num1 / num2).ToString(); // Если "/", делим 
                    break;
            }
        }
    }
}