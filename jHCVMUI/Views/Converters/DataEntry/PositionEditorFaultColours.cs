using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jHCVMUI.Views.Converters.DataEntry
{
  using System.Windows.Media;

  /// <summary>
  /// Library of the colours used to represent the various states on the position editor dialog.
  /// </summary>
  public static class PositionEditorFaultColours
  {
    public static Color NoFaultColour => Colors.Transparent;

    public static Color DoubleRead => Colors.Red;

    public static Color DoubleReadNone => Colors.Pink;

    public static Color Duplicate => Colors.Orange;

    public static Color DuplicateNone => Colors.PeachPuff;

    public static Color Missing => Colors.Gold;

    public static Color MissingNone => Colors.LightYellow;

    public static Color NumberNotRecognised => Colors.DodgerBlue;

    public static Color NumberNotRecognisedNone => Colors.LightBlue;

    public static Color MissingPosition => Colors.MediumOrchid;

    public static Color MissingPositionNone => Colors.Plum;
  }
}
