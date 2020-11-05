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

  [ValueConversion(typeof(PositionEditorFaults), typeof(Brush))]
  public class PositionEditorRowBackgroundConverter : IValueConverter
  {
    public object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      Color returnColour = Colors.Transparent;

        if (value == null || value.GetType() != typeof(PositionEditorFaults))
      {
        return new SolidColorBrush(returnColour);
      }

      PositionEditorFaults fault = (PositionEditorFaults)value;

      switch (fault)
      {
        case PositionEditorFaults.DoubleRead:
          returnColour = PositionEditorFaultColours.DoubleRead;
          break;
        case PositionEditorFaults.Duplicate:
          returnColour = PositionEditorFaultColours.Duplicate;
          break;
        case PositionEditorFaults.Missing:
          returnColour = PositionEditorFaultColours.Missing;
          break;
        case PositionEditorFaults.NumberNotRecognised:
          returnColour = PositionEditorFaultColours.NumberNotRecognised;
          break;
      }

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
