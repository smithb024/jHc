namespace jHCVMUI.Views.Converters.Configuration
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows.Data;
  using System.Windows.Media;
  using CommonLib.Enumerations;

  [ValueConversion(typeof(FieldUpdatedType), typeof(Color))]
  public class FieldUpdatedToColourConverter : IValueConverter
  {
    // TODO, is this really necessary?
    public enum LocalStatus
    {
      Unchanged,
      Changed,
      Invalid,
      Disabled
    }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      FieldUpdatedType state = (FieldUpdatedType)value;
      LocalStatus localState = (LocalStatus)Enum.Parse(typeof(LocalStatus), value.ToString());

      switch (state)
      {
        case (FieldUpdatedType.Unchanged):
          if (localState == LocalStatus.Unchanged)
          {
            return new SolidColorBrush(Colors.Black);
          }
          break;
        case (FieldUpdatedType.Changed):
          if (localState == LocalStatus.Changed)
          {
            return new SolidColorBrush(Colors.DarkSeaGreen);
          }
          break;
        case (FieldUpdatedType.Invalid):
          if (localState == LocalStatus.Invalid)
          {
            return new SolidColorBrush(Colors.IndianRed);
          }
          break;
        case (FieldUpdatedType.Disabled):
          if (localState == LocalStatus.Disabled)
          {
            return new SolidColorBrush(Colors.WhiteSmoke);
          }
          break;
      }

      return new SolidColorBrush(Colors.Gray);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return null;
    }
  }
}