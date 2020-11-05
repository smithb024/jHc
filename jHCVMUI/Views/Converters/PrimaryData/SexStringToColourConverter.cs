namespace jHCVMUI.Views.Converters.PrimaryData
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows.Data;
  using System.Windows.Media;
  using CommonHandicapLib.Types;

  /// <summary>
  /// Used to convert a best (personal or season) to a highlighted backcolour.
  /// </summary>
  [ValueConversion(typeof(string), typeof(Brush))]
  public class SexStringToColourConverter : IValueConverter
  {

    public object Convert(
      object value,
      Type targetType,
      object parameter,
      System.Globalization.CultureInfo culture)
    {
      if (value == null || value.GetType() != typeof(string))
      {
        return new SolidColorBrush(Colors.Transparent);
      }

      string sex = (string)value;

      if (string.Compare(sex, "Male") == 0)
      {
        return new SolidColorBrush(Colors.LightCyan);
      }
      else if (string.Compare(sex, "Female") == 0)
      {
        return new SolidColorBrush(Colors.LightPink);
      }

      return new SolidColorBrush(Colors.Transparent);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}