using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jHCVMUI.Views.Converters.DataEntry
{
  using System.Globalization;
  using System.Windows.Data;
  using System.Windows.Media;

  using CommonHandicapLib.Types;

  [ValueConversion(typeof(int), typeof(Brush))]
  public class ErrorBarForegroundConverter : IValueConverter
  {
    public object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      if (value == null || value.GetType() != typeof(int))
      {
        return new SolidColorBrush(Colors.Transparent);
      }

      Color returnColour =
        (int)value > 0 ?
        Colors.Black :
        Colors.Gray;

        return new SolidColorBrush(returnColour);
    }

    /// <summary>
    /// Not used.
    /// </summary>
    /// <param name="value">value not used</param>
    /// <param name="targetType">target type is not used</param>
    /// <param name="parameter">parameter is not used</param>
    /// <param name="culture">culture is not used</param>
    /// <returns>returns not used</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}