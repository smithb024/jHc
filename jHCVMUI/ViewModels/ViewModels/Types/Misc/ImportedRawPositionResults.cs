namespace jHCVMUI.ViewModels.ViewModels.Types.Misc
{
  /// <summary>
  /// Stores the position and race number details as imported from a te=xt file.
  /// </summary>
  public class ImportedRawPositionResults : ViewModelBase
  {
    /// <summary>
    /// Initialises a new instance of the <see cref="ImportedRawPositionResults"/> class.
    /// </summary>
    /// <param name="raceNumber"></param>
    /// <param name="position"></param>
    public ImportedRawPositionResults(string raceNumber, int position)
    {
      RaceNumber = raceNumber;
      Position   = position;
    }

    /// <summary>
    /// Gets the race number.
    /// </summary>
    public string RaceNumber
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the position of the race number.
    /// </summary>
    public int Position
    {
      get;
      private set;
    }
  }
}