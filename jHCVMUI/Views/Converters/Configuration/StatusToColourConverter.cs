namespace jHCVMUI.Views.Converters.Configuration
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows.Data;
  using System.Windows.Media;
  using CommonHandicapLib.Types;

  [ValueConversion(typeof(StatusType), typeof(Color))]
  public class StatusToColourConverter : IValueConverter
  {

  public enum LocalStatus
  {
    Ok,
    Added,
    Updated,
    Deleted
  }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      StatusType state = (StatusType)value;
      LocalStatus localState = (LocalStatus)Enum.Parse(typeof(LocalStatus), value.ToString());

      switch (state)
      {
        case (StatusType.Added):
          if (localState == LocalStatus.Added)
          {
            return new SolidColorBrush(Colors.Green);
          }
          break;
        case (StatusType.Deleted):
          if (localState == LocalStatus.Deleted)
          {
            return new SolidColorBrush(Colors.Red);
          }
          break;
        case (StatusType.Ok):
          if (localState == LocalStatus.Ok)
          {
            return new SolidColorBrush(Colors.Black);
          }
          break;
        case (StatusType.Updated):
          if (localState == LocalStatus.Updated)
          {
            return new SolidColorBrush(Colors.Blue);
          }
          break;
      }
      return new SolidColorBrush(Colors.Black);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return null;
    }
  }
}
