namespace jHCVMUI.Views.Converters.Common
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
  [ValueConversion(typeof(bool), typeof(Color))]
  public class HighlightForegroundColourConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      bool testValue = (bool)value;

      return testValue ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Maroon);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}