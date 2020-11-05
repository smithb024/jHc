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
  [ValueConversion(typeof(ResultsState), typeof(Color))]
  public class BestToColourConverter : IValueConverter
  {
    /// <summary>
    /// Describes the table status
    /// </summary>
    public enum LocalStatus
    {
      DefaultState,
      NewPB,
      NewSB,
      FirstTimer
    }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      ResultsState state = (ResultsState)value;
      LocalStatus localState = (LocalStatus)Enum.Parse(typeof(LocalStatus), value.ToString());

      switch (state)
      {
        case (ResultsState.NewPB):
          if (localState == LocalStatus.NewPB)
          {
            return new SolidColorBrush(Colors.Honeydew);
          }
          break;
        case (ResultsState.NewSB):
          if (localState == LocalStatus.NewSB)
          {
            return new SolidColorBrush(Colors.LightCyan);
          }
          break;
        case (ResultsState.FirstTimer):
          if (localState == LocalStatus.FirstTimer)
          {
            return new SolidColorBrush(Colors.LemonChiffon);
          }
          break;
      }
      return new SolidColorBrush(Colors.Transparent);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}