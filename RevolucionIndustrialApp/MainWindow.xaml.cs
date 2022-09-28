using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace RevolucionIndustrialApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Button_Play.Click += (s, e) => { this.ParseUserInput(); };
        }


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Console.WriteLine("asdf");
        }

        private void ParseUserInput()
        {
            string userInput = this.TextBox_Date.Text.ToString();
            
            try
            {
                ValidateUserInput(userInput);
                DateTime userDate = GetUserDate(userInput);
                ValidateYearWithinRange(userDate);

                this.Slider_Year.Value = userDate.Year;
                ValidateYearWithinRevolutionRange(userDate);
                ValidateDateWithinPresentDayRange(userDate);
                SetDateMessage(userDate);

            }
            catch (ArgumentException aException)
            {
                this.TextBox_Date.BorderBrush = System.Windows.Media.Brushes.Red;
                this.TextBlock_InputError.Text=aException.Message;            
            }
        }

        private void SetDateMessage(DateTime userDate) 
        {
            this.TextBlock_RevolucionDateMessage.Text = "";
            SetRevolucion1DateMessage(userDate);
            SetRevolucion2DateMessage(userDate);
            SetRevolucion3DateMessage(userDate);
            SetRevolucion4DateMessage(userDate);
            SetPeriodoTransitorioDateMessage(userDate);
        }

        private void SetRevolucion1DateMessage(DateTime userDate) 
        {
            int minimum = (int)RevolucionIntervals.Revolucion1_MinimumYear;
            int maximum = (int)RevolucionIntervals.Revolucion1_MaximumYear;
            if (minimum <= userDate.Year && userDate.Year <= maximum)
            { 
                this.TextBlock_RevolucionDateMessage.Text = Resource.ResourceManager.GetString("General_TextBlock_Industria1.0");
            }
        }


        private void SetRevolucion2DateMessage(DateTime userDate)
        {
            int minimum = (int)RevolucionIntervals.Revolucion2_MinimumYear;
            int maximum = (int)RevolucionIntervals.Revolucion2_MaximumYear;
            if (minimum <= userDate.Year && userDate.Year <= maximum)
            {
                this.TextBlock_RevolucionDateMessage.Text = Resource.ResourceManager.GetString("General_TextBlock_Industria2.0");
            }
        }

        private void SetRevolucion3DateMessage(DateTime userDate)
        {
            int minimum = (int)RevolucionIntervals.Revolucion3_MinimumYear;
            int maximum = (int)RevolucionIntervals.Revolucion3_MaximumYear;
            if (minimum <= userDate.Year && userDate.Year <= maximum)
            {
                this.TextBlock_RevolucionDateMessage.Text = Resource.ResourceManager.GetString("General_TextBlock_Industria3.0");
            }
        }


        private void SetRevolucion4DateMessage(DateTime userDate)
        {
            int minimum = (int)RevolucionIntervals.Revolucion4_MinimumYear;
            int maximum = (int)RevolucionIntervals.Revolucion4_MaximumYear;
            if (minimum <= userDate.Year && userDate.Year <= maximum)
            {
                this.TextBlock_RevolucionDateMessage.Text = Resource.ResourceManager.GetString("General_TextBlock_Industria4.0");
            }
        }

        private void SetPeriodoTransitorioDateMessage(DateTime userDate)
        {
            string currentDateMessage = this.TextBlock_RevolucionDateMessage.Text;
            if (String.IsNullOrEmpty(currentDateMessage))
            {
                this.TextBlock_RevolucionDateMessage.Text = Resource.ResourceManager.GetString("General_TextBlock_PeriodoTransitorio");
            }
        }
        private DateTime GetUserDate(string userInput) 
        {
            DateTime userDate = new DateTime(
                GetYearFromInput(userInput),
                GetMonthFromInput(userInput),
                GetDayFromInput(userInput)
            );
            return userDate;
        }

        private void ValidateDateWithinPresentDayRange(DateTime userDate)
        {
            DateTime currentDate = DateTime.Now;
            ValidateUserYearWithinPresentYear(userDate, currentDate);
        }

        private void ValidateUserYearWithinPresentYear(DateTime userDate, DateTime currentDate)
        {
            if (userDate.Year > currentDate.Year)
            {
                throw new ArgumentException("Ups! No sé adivinar el futuro.");
            }
            else
            {
                if (!(userDate.Year < currentDate.Year))
                {
                    ValidateUserMonthWithinPresentMonth(userDate, currentDate);
                }
               
            }
        }

        private void ValidateUserMonthWithinPresentMonth(DateTime userDate, DateTime currentDate)
        {

            if (userDate.Month > currentDate.Month)
            {
                throw new ArgumentException("Ups! No sé adivinar el futuro.");
            }
            else
            {
                ValidateUserDayWithinPresentDay(userDate, currentDate);
            }
            

        }

        private void ValidateUserDayWithinPresentDay(DateTime userDate, DateTime currentDate)
        {
            if (userDate.Day > currentDate.Day)
            { 
                throw new ArgumentException("Ups! No sé adivinar el futuro.");
            }
        }

        private void ValidateYearWithinRevolutionRange(DateTime userDate) 
        {
            int revolutionYearMinimum = 1760;
            if (userDate.Year < revolutionYearMinimum) 
            {
                throw new ArgumentException("Ups! La industria todavía no surgía como tal.");
            }
        }

        private void ValidateYearWithinRange(DateTime userDate) 
        { 
            int yearMinimum = 1700;
            int yearMaximum = 2100;
            if (userDate.Year < yearMinimum || userDate.Year > yearMaximum)
            {
                throw new ArgumentException("Ups! Fuera de rango");
            }
        }

        private void ValidateUserInput(String userInput)
        {
            Regex validDateRegex = new Regex(
                "^(0?[1-9]|[12][0-9]|3[01])[\\/\\-](0?[1-9]|1[012])[\\/\\-]\\d{4}$"
            );
            bool inputIsValidDate = validDateRegex.IsMatch(userInput);
            if (!inputIsValidDate)
            {
                throw new ArgumentException("Ups! Intenta de nuevo");
            }
        }

        private void TextBox_Date_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.TextBox_Date.BorderBrush = System.Windows.Media.Brushes.LightGray;
            this.TextBlock_InputError.Text = "";
            this.Popup_Alert.IsOpen = false;
            this.Popup_Alert.HorizontalOffset = 0;
        }

        private void Slider_Year_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private int GetDayFromInput(String userInput)
        {
            string dayString = "";
            dayString += userInput[0];
            dayString += userInput[1];
            int day = Int32.Parse(dayString);
            return day;
        }

        private int GetMonthFromInput(String userInput)
        {
            string monthString = "";
            monthString += userInput[3];
            monthString += userInput[4];
            int month = Int32.Parse(monthString);
            return month;
        }

        private int GetYearFromInput(String userInput)
        {
            string yearString = ""; 
            yearString += userInput[6];
            yearString += userInput[7];
            yearString += userInput[8];
            yearString += userInput[9];
            int year = Int32.Parse(yearString);
            return year;
        }
    }
}
