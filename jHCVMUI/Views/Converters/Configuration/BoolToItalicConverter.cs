namespace jHCVMUI.Views.Converters.Configuration
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows.Data;

  [ValueConversion(typeof(bool), typeof(System.Windows.FontStyle))]
  public class BoolToItalicConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return (bool)value ? System.Windows.FontStyles.Normal : System.Windows.FontStyles.Italic;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}