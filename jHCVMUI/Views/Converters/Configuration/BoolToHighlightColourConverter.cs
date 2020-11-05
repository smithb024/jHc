namespace jHCVMUI.Views.Converters.Configuration
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows.Data;
  using System.Windows.Media;

  [ValueConversion(typeof(bool), typeof(Color))]
  public class BoolToHighlightColourConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return (bool)value ? new SolidColorBrush(Colors.Navy) : new SolidColorBrush(Colors.Transparent);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}