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
  public class ErrorBarBackgroundConverter : IValueConverter
  {
    /// <summary>
    /// Initialises a new instance of the <see cref="ErrorBarBackgroundConverter"/> class.
    /// </summary>
    public ErrorBarBackgroundConverter()
    {
      this.Fault = PositionEditorFaults.NoFault;
    }

    /// <summary>
    /// Gets or sets the fault which this converter represents.
    /// </summary>
    public PositionEditorFaults Fault { get; set; }

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
        this.GetNonZeroColour() :
        this.GetZeroColour();

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

    /// <summary>
    /// Gets a colour which depends on <see cref="Fault"/>.
    /// </summary>
    /// <returns>new colour.</returns>
    private Color GetZeroColour()
    {
      switch (this.Fault)
      {
        case PositionEditorFaults.DoubleRead:
          return PositionEditorFaultColours.DoubleReadNone;
        case PositionEditorFaults.Duplicate:
          return PositionEditorFaultColours.DuplicateNone;
        case PositionEditorFaults.Missing:
          return PositionEditorFaultColours.MissingNone;
        case PositionEditorFaults.MissingPositionToken:
          return PositionEditorFaultColours.MissingPositionNone;
        case PositionEditorFaults.NumberNotRecognised:
          return PositionEditorFaultColours.NumberNotRecognisedNone;
        default:
          return Colors.Transparent;
      }
    }

    /// <summary>
    /// Gets a colour which depends on <see cref="Fault"/>.
    /// </summary>
    /// <returns>new colour.</returns>
    private Color GetNonZeroColour()
    {
      switch (this.Fault)
      {
        case PositionEditorFaults.DoubleRead:
          return PositionEditorFaultColours.DoubleRead;
        case PositionEditorFaults.Duplicate:
          return PositionEditorFaultColours.Duplicate;
        case PositionEditorFaults.Missing:
          return PositionEditorFaultColours.Missing;
        case PositionEditorFaults.MissingPositionToken:
          return PositionEditorFaultColours.MissingPosition;
        case PositionEditorFaults.NumberNotRecognised:
          return PositionEditorFaultColours.NumberNotRecognised;
        default:
          return Colors.Transparent;
      }
    }
  }
}
