namespace CommonHandicapLib.Types
{
  /// <summary>
  /// Class contains all the configuration information used to with the series.
  /// </summary>
  public class SeriesConfigType
  {
    /// <summary>
    /// Initialises a new instance of the <see cref="SeriesConfigType"/> class.
    /// </summary>
    /// <param name="minimumHandicap">minimum available handicap</param>
    public SeriesConfigType(
      string numberPrefix,
      bool allPositions)
    {
      this.NumberPrefix = numberPrefix;
      this.AllPositionsShown = allPositions;
    }

    /// <summary>
    /// Gets the string which can't be at the beginning of the number.
    /// </summary>
    private static string ProtectedString => "P";

    public static bool ValidNumber(string testNumber)
    {
      return !(testNumber?.Length > 0 &&
        string.Compare(testNumber.Substring(0, 1), SeriesConfigType.ProtectedString, true) == 0);
    }

    /// <summary>
    /// Gets or sets the number prefix.
    /// </summary>
    public string NumberPrefix
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets a value which indicates if all positions should be calculated.
    /// </summary>
    public bool AllPositionsShown { get; set; }
  }
}