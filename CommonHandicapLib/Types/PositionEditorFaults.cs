using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHandicapLib.Types
{
  /// <summary>
  /// Enumeration describing the faults which are posible.
  /// </summary>
  public enum PositionEditorFaults
  {
    /// <summary>
    /// There is no fault
    /// </summary>
    NoFault,

    /// <summary>
    /// A barcode has been read twice.
    /// </summary>
    DoubleRead,

    /// <summary>
    /// The number or position is duplicated elsewhere in the raw results.
    /// </summary>
    Duplicate,

    /// <summary>
    /// A number or position is missing.
    /// </summary>
    /// <remarks>
    /// Number/position should be read in pairs, if odd line is position, even should be race 
    /// number (and vice versa). This highlights any which don't follow the pattern.
    /// </remarks>
    Missing,

    /// <summary>
    /// The race number is not recognised.
    /// </summary>
    NumberNotRecognised,

    /// <summary>
    /// There is a position missing from the series.
    /// </summary>
    MissingPositionToken
  }
}