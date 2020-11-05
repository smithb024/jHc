namespace CommonHandicapLib.Types
{
  using System;

  /// <summary>
  /// Class contains all the configuration information used to work out the handicaps.
  /// </summary>
  public class NormalisationConfigType
  {
    /// <summary>
    /// Initialises a new instance of the <see cref="NormalisationConfigType"/> class.
    /// </summary>
    /// <param name="useHandicap">the application uses handicaps or not</param>
    /// <param name="handicapTime">time in minutes the handicap is measured against</param>
    /// <param name="minimumHandicap">minimum available handicap</param>
    /// <param name="handicapInterval">interval between handicap times</param>
    public NormalisationConfigType(
      bool useHandicap,
      int  handicapTime,
      int  minimumHandicap,
      int handicapInterval)
    {
      UseCalculatedHandicap     = useHandicap;
      HandicapTime    = handicapTime;
      MinimumHandicap = minimumHandicap;
      this.HandicapInterval = handicapInterval;
    }

    /// <summary>
    /// Gets or sets a flag indicating if the application uses a self calculated handicap or it
    /// uses a predefined one.
    /// </summary>
    public bool UseCalculatedHandicap
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the time the handicap is measured against in minutes.
    /// </summary>
    public int HandicapTime
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the minimum possible handicap in minutes.
    /// </summary>
    public int MinimumHandicap
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the interval between handicaps on .
    /// </summary>
    public int HandicapInterval
    {
      get;
      set;
    }
  }
}