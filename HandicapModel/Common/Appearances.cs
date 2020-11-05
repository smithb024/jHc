namespace HandicapModel.Common
{
  using CommonHandicapLib.Types;
  using CommonLib.Types;

  /// <summary>
  /// Records the date and run time of a specific appearance.
  /// </summary>
  public class Appearances
  {
    private RaceTimeType time = new RaceTimeType();
    private DateType     date = new DateType();

    /// <summary>
    /// Initialises a new instance of the AppearanceType class.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="date"></param>
    public Appearances(RaceTimeType time,
                       DateType     date)
    {
      Time = time;
      Date = date;
    }

    /// <summary>
    /// Gets and sets the appearance time
    /// </summary>
    public RaceTimeType Time
    {
      get { return time; }
      set { time = value; }
    }

    /// <summary>
    /// Gets the appearance time as a string.
    /// </summary>
    public string TimeString => this.time.ToString();

    /// <summary>
    /// Gets and sets the appearance date.
    /// </summary>
    public DateType Date
    {
      get { return date; }
      set { date = value; }
    }

    /// <summary>
    /// Gets the appearance date as a string.
    /// </summary>
    public string DateString => this.date.ToString();
  }
}
