namespace TempratureConverter.Services;
//a class that holds all the conversion methods using 
//Expression-Bodied Members which is
// one liner method with lambda operator 
//CONVERSION OPTIONS
//Celsius <==> Fahrenheit | Kelvin <==> Celsius | Fahrenheit<==>Kelvin 
public class TempConverter
{
  public double C_TO_F(double celsius) => celsius * (9d / 5d) + 32;

  public double F_TO_C(double fahrenheit) => (fahrenheit - 32) * 5d / 9d;

  public double C_TO_K(double celsius) => celsius + 273.15;

  public double K_TO_C(double kelvin) => kelvin - 273.15;

  public double F_TO_K(double fahrenheit) => C_TO_K(F_TO_C(fahrenheit));

  public double K_TO_F(double celsius) => C_TO_F(K_TO_C(celsius));
}