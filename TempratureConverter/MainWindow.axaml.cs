using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using TempratureConverter.Services;

namespace TempratureConverter;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        OpenOnSecondDisplayMonitor();
    }

    //function that always opens the App-Window on Popup on the Secondary Display
    //because primary display is used for Coding
    private void OpenOnSecondDisplayMonitor()
    {
        var screens = Screens.All;
        if (screens.Count > 1)
        {
            var secondDisplay = screens[1];
            Position = new PixelPoint(secondDisplay.WorkingArea.X, secondDisplay.WorkingArea.Y);
        }
    }

    //function that makes the input textbox Number Only, by
    //by checking on each change in value 
    //if the value is int or double
    //if not delete the last typed key
    private void Input_OnChange_FilterNumbers(object? sender, TextChangedEventArgs e)
    {

        if (CelsiusInputVal.Text?.Length > 0 && CelsiusInputVal.Text is not null)
        {
            string input = CelsiusInputVal.Text;
            //Proper way to handle unexpected errors
            //try to parse value to double if failed means non number char was typed 
            //then we delete it with range operator 
            if (!double.TryParse(input, out _))
            {
                CelsiusInputVal.Text = input[..^1];//remove the ":" from the unit string value using RANGE OPERATOR 
            }
        }

    }
    //Expression-Bodied Members which is
    // one liner method with lambda operator 
    //that takes ComboBoxItem values to dynamically create unique ComboBoxItems
    //arg1 is the content or Text or temp unit string
    //arg2 is if the ComboBoxItem should be enabled or disable for selection
    // disable only when the wanted ComboBoxItem is the source temp unit string
    //the method return type is ComboBoxItem
    // written with the new shorter syntax: "new()" instead of  "new ComboBoxItem()"
    private static ComboBoxItem GenComboBoxItem(object content, bool isEnbled) => new(){Content = content,IsEnabled = isEnbled};

    //BUTTON that changes the source and target temprature units
    private void Btn_OnClick_ChangeUnit(object? sender, RoutedEventArgs e)
    {
        //make sure that the source value isnt empty nor null
        if (SourceUnit.Text?.Length > 0 && SourceUnit.Text != null)
        {
            //Strings array that represents the current order 
            //[0] is the selected one [1] and [2] are the textbox other options
            string[] units = ["Celsius", "Fahrenheit", "Kelvin"];
            string currentSource = SourceUnit.Text;
            currentSource = currentSource[..^1];//remove the ":" from the unit string value using RANGE OPERATOR 

            //find the current source index in units array
            //to use it to reorder the source unit and target unit -tempratures
            int sourceIndx = Array.IndexOf(units, currentSource);

            if (sourceIndx == 0)
                units = ["Fahrenheit", "Kelvin", "Celsius"];
            else if (sourceIndx == 1)
                units = ["Kelvin", "Celsius", "Fahrenheit"];
            // else if sourceIndx == 2 leave as IS

            //now clear the ComboBox items List
            //then refill with updated values
            ConvertUnitDD.Items.Clear();

            //fill ComboBox items List
            //using GenComboBoxItem custom built func
            ConvertUnitDD.Items.Add(GenComboBoxItem(units[1],true));
            ConvertUnitDD.Items.Add(GenComboBoxItem(units[2],true));
            ConvertUnitDD.Items.Add(GenComboBoxItem(units[0],false));

           //now make the selected combox item the first one in the items list (ConvertUnitDD.Items.Add(GenComboBoxItem(units[1],true));)
            ConvertUnitDD.SelectedIndex = 0;
            //now add the ":" for the source unit title in the UI
            SourceUnit.Text = units[0] + ":";

        }
    }


    //Button that runs the calculatio
    private void Btn_OnClick_Calc(object? sender, RoutedEventArgs e)
    {   
        //class that contains 6 Expression-Bodied Members methods with lambda operator
        //Celsius <==> Fahrenheit | Kelvin <==> Celsius | Fahrenheit<==>Kelvin 
        // written with the new shorter syntax: "new()" instead of  "new TempConverter()"
        TempConverter tempConverter = new();

        //make sure that the source value and CelsiusInputVal isnt empty nor null
        if (SourceUnit.Text?.Length > 0 && CelsiusInputVal.Text?.Length > 0 && SourceUnit.Text != null)
        {
            string sourceUnit = SourceUnit.Text;
            sourceUnit = sourceUnit[..^1];//remove the ":" from the unit string value using RANGE OPERATOR 
            var item = ConvertUnitDD.SelectedItem as ComboBoxItem;//return the current selected item in the combobox
            string? targetUnit = item?.Content?.ToString();//extract the text or value of current selected item in the combobox
            double sourceTemp = double.Parse(CelsiusInputVal.Text);//parse the value  that we wanna convert 

            // Switch that runs the correct conversion methods 
            //accroding to the sourceUnit + targetUnit strings concatinated togther
            //then it updates the Result in the target temprature in the UI
            //we round the values to max of 2 digits after the int value in the double to fit ALWAYS in the UI
            switch (sourceUnit + targetUnit)
            {
                case "CelsiusFahrenheit":
                    TargetTemp.Text = Math.Round(tempConverter.C_TO_F(sourceTemp), 2) + " " + "F°";
                    break;
                case "CelsiusKelvin":
                    TargetTemp.Text = Math.Round(tempConverter.C_TO_K(sourceTemp), 2) + " " + "°K";
                    break;
                case "FahrenheitCelsius":
                    TargetTemp.Text = Math.Round(tempConverter.F_TO_C(sourceTemp), 2) + " " + "°C";
                    break;
                case "FahrenheitKelvin":
                    TargetTemp.Text = Math.Round(tempConverter.F_TO_K(sourceTemp), 2) + " " + "°K";
                    break;
                case "KelvinCelsius":
                    TargetTemp.Text = Math.Round(tempConverter.K_TO_C(sourceTemp), 2) + " " + "°C";
                    break;
                case "KelvinFahrenheit":
                    TargetTemp.Text = Math.Round(tempConverter.K_TO_F(sourceTemp), 2) + " " + "F°";
                    break;
            }
        }
    }
}