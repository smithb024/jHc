using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jHCVMUI.ViewModels.DataEntry
{
  using CommonHandicapLib.Helpers.EventRawResults;
  using CommonHandicapLib.Types;

  using jHCVMUI.ViewModels.ViewModels;

  public class PositionEditorRawItem : ViewModelBase
  {
    /// <summary>
    /// Records the fault present on this <see cref="PositionEditorRawItem"/>;
    /// </summary>
    private PositionEditorFaults fault;

    /// <summary>
    /// Initialises a new instance of the <see cref="PositionEditorRawItem"/> class
    /// </summary>
    /// <param name="input"></param>
    public PositionEditorRawItem(string input)
    {
      this.Complete = input;
      this.Barcode = ResultsDecoder.OpnScannerResultsBarcode(input);
      this.ExtraInformation = ResultsDecoder.OpnScannerResultsOtherData(input);
      this.fault = PositionEditorFaults.NoFault;

      int defaultPosition = -1;

      // Calculate the position as an interger. Use default if there is a fault in the calculation,
      // or a position is not represented.
      if (this.Type == PositionEditorType.Position)
      {
        int position;
        int.TryParse(ResultsDecoder.GetPositionNumber(this.Barcode), out position);

        if (position != 0)
        {
          this.Position = position;
        }
        else
        {
          this.Position = defaultPosition;
        }
      }
      else
      {
        this.Position = defaultPosition;
      }
    }

    /// <summary>
    /// Gets a value which indicates whether this data is a position or a race number.
    /// </summary>
    public PositionEditorType Type =>
      ResultsDecoder.IsPositionValue(this.Barcode) ?
      PositionEditorType.Position :
      PositionEditorType.RaceNumber;

    /// <summary>
    /// Gets the bar code information.
    /// </summary>
    public string Barcode { get; }

    /// <summary>
    /// Gets the extra information on the input string.
    /// </summary>
    public string ExtraInformation { get; }

    /// <summary>
    /// Gets the whole input string.
    /// </summary>
    public string Complete { get; }

    /// <summary>
    /// Gets or sets the fault identified for this <see cref="PositionEditorRawItem"/>.
    /// </summary>
    public PositionEditorFaults Fault { get
      {
        return this.fault;
      }

      set
      {
        this.fault = value;
        this.RaisePropertyChangedEvent(nameof(this.Fault));
      }
    }

    /// <summary>
    /// Gets the position that this barcode represents. 
    /// </summary>
    /// <remarks>
    /// Negative number is returned if this doesn't represent a position.
    /// </remarks>
    public int Position { get; }
  }
}
