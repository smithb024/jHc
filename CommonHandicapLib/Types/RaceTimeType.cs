namespace CommonHandicapLib.Types
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using CommonLib.Types;

  /// <summary>
  /// Class describing the time taken to complete an event.
  /// </summary>
  /// <remarks>
  /// Includes the flag did not finshed.
  /// </remarks>
  public class RaceTimeType : TimeType
  {
    /// <summary>
    /// The athlete started but did not finish.
    /// </summary>
    private const string dnfDescription = "DNF";

    /// <summary>
    /// The time is not known.
    /// </summary>
    private const string unknownDescription = "UNK";

    private bool dnf = false;
    private bool unknown = false;

    /// <summary>
    /// Initialises a new instance of <see cref="RaceTimeType"/> type.
    /// </summary>
    /// <param name="time">new time</param>
    public RaceTimeType(string time)
    {
      if (time.CompareTo(dnfDescription) == 0)
      {
        DNF = true;
      }
      else if (time.CompareTo(unknownDescription) == 0)
      {
        Unknown = true;
      }
      else
      {
        Update(time);
      }
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="TaceTimeType"/> class.
    /// </summary>
    /// <param name="minutes">new minutes</param>
    /// <param name="seconds">new seconds</param>
    public RaceTimeType(int minutes, int seconds)
      : base(minutes, seconds)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="RaceTimeType"/> type.
    /// </summary>
    /// <remarks>
    /// Contains DNF value, you would only expect a true in this constructor.
    /// </remarks>
    /// <param name="dnf">did not finish value</param>
    /// <param name="unknown">race time is unknown</param>
    public RaceTimeType(
      bool dnf,
      bool unknown)
    {
      this.DNF = dnf;
      this.Unknown = unknown;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="RaceTimeType"/> class.
    /// </summary>
    /// <remarks>
    /// Contains default values.
    /// </remarks>
    public RaceTimeType()
    {
    }

    /// <summary>
    /// Gets or sets a flag indicating if the athlete finished or not.
    /// </summary>
    public bool DNF
    {
      get { return dnf; }
      set { dnf = value; }
    }

    /// <summary>
    /// Gets or sets a flag indicating if the time is known or not.
    /// </summary>
    public bool Unknown
    {
      get { return unknown; }
      set { unknown = value; }
    }

    /// <summary>
    /// Add time, if seconds role over to the next  minute increase minute by one and reset seconds.
    /// </summary>
    /// <param name="lhs">time to add (left)</param>
    /// <param name="rhs">time to add (right)</param>
    /// <returns>new time</returns>
    public static RaceTimeType operator +(RaceTimeType lhs,
                                          RaceTimeType rhs)
    {
      if (lhs.DNF == true || rhs.DNF == true)
      {
        return new RaceTimeType(true, false);
      }

      if (lhs.Unknown == true || rhs.Unknown == true)
      {
        return new RaceTimeType(false, true);
      }

      int productMinutes  = lhs.Minutes + rhs.Minutes;
      int productSeconds  = lhs.Seconds + rhs.Seconds;

      if (productSeconds >= 60)
      {
        ++productMinutes;
        productSeconds = productSeconds - 60;
      }

      return new RaceTimeType(productMinutes, productSeconds);
    }

    /// <summary>
    /// Subtract time, if seconds role over to the next minute decrease minute by one and reset seconds.
    /// </summary>
    /// <param name="lhs">distance to add (left)</param>
    /// <param name="rhs">distance to add (right)</param>
    /// <returns>new distance</returns>
    public static RaceTimeType operator -(RaceTimeType lhs,
                                          RaceTimeType rhs)
    {
      if (lhs.DNF == true || rhs.DNF == true)
      {
        return new RaceTimeType(true, false);
      }

      if (lhs.Unknown == true || rhs.Unknown == true)
      {
        return new RaceTimeType(false, true);
      }

      if (rhs.Minutes > lhs.Minutes ||
        (rhs.Minutes == lhs.Minutes && rhs.Seconds > lhs.Seconds))
      {
        return new RaceTimeType(0, 0);
      }
      else
      {
        int productMinutes = lhs.Minutes - rhs.Minutes;
        int productSeconds = lhs.Seconds - rhs.Seconds;

        if (productSeconds < 0)
        {
          --productMinutes;
          productSeconds = productSeconds + 60;
        }

        return new RaceTimeType(productMinutes, productSeconds);
      }
    }

    /// <summary>
    /// Overrides the equals operator to check the minutes and seconds match
    /// </summary>
    /// <param name="obj">object to compare</param>
    /// <returns>true or false</returns>
    public override bool Equals(object obj)
    {
      if (obj == null)
      {
        return false;
      }

      // Ensure that the obj can be cast to race time type.
      RaceTimeType time = obj as RaceTimeType;
      if ((System.Object)time == null)
      {
        return false;
      }

      return 
        (time.DNF && DNF) ||
        (time.Minutes == Minutes && time.Seconds == Seconds && time.DNF == DNF) ||
        (time.Unknown && Unknown) ||
        (time.Minutes == Minutes && time.Seconds == Seconds && time.Unknown == Unknown);
    }

    /// <summary>
    /// Overrides the get hash code method
    /// </summary>
    /// <returns>hash code</returns>
    public override int GetHashCode()
    {
      return Minutes ^ Seconds;
    }

    /// <summary>
    /// equality operator
    /// </summary>
    /// <param name="lhs">left hand side</param>
    /// <param name="rhs">right hand side</param>
    /// <returns>equality flag</returns>
    public static bool operator ==(RaceTimeType lhs,
                                   RaceTimeType rhs)
    {
      if (object.ReferenceEquals(lhs, null))
      {
        return object.ReferenceEquals(rhs, null);
      }

      if (object.ReferenceEquals(rhs, null))
      {
        return false;
      }

      return
        (rhs.DNF && lhs.DNF) ||
        (lhs.Minutes == rhs.Minutes && lhs.Seconds == rhs.Seconds && lhs.DNF == rhs.DNF) ||
        (rhs.Unknown && lhs.Unknown) ||
        (lhs.Minutes == rhs.Minutes && lhs.Seconds == rhs.Seconds && lhs.Unknown == rhs.Unknown);
    }

    /// <summary>
    /// inequality operator
    /// </summary>
    /// <param name="lhs">left hand side</param>
    /// <param name="rhs">right hand side</param>
    /// <returns>inequality flag</returns>
    public static bool operator !=(RaceTimeType lhs,
                                   RaceTimeType rhs)
    {
      if (object.ReferenceEquals(lhs, null))
      {
        return !object.ReferenceEquals(rhs, null);
      }

      if (object.ReferenceEquals(rhs, null))
      {
        return true;
      }

      return (lhs.Minutes != rhs.Minutes || lhs.Seconds != rhs.Seconds || rhs.DNF != lhs.DNF || rhs.Unknown != lhs.Unknown);
    }

    /// <summary>
    /// Checks to see if one is larger than the other. It returns false if either is DNF.
    /// </summary>
    /// <param name="lhs">left hand side</param>
    /// <param name="rhs">right hand side</param>
    /// <returns>size indicator flag</returns>
    public static bool operator >(RaceTimeType lhs,
                                  RaceTimeType rhs)
    {
      if (lhs.DNF == true || rhs.DNF == true)
      {
        return false;
      }

      if (lhs.Unknown == true || rhs.Unknown == true)
      {
        return false;
      }

      if (lhs.Minutes > rhs.Minutes)
      {
        return true;
      }
      else if (lhs.Minutes == rhs.Minutes && lhs.Seconds > rhs.Seconds)
      {
        return true;
      }

      return false;
    }

    /// <summary>
    /// Checks to see if one is smaller than the other. It returns false if either is DNF.
    /// </summary>
    /// <param name="lhs">left hand side</param>
    /// <param name="rhs">right hand side</param>
    /// <returns>size indicator flag</returns>
    public static bool operator <(RaceTimeType lhs,
                                  RaceTimeType rhs)
    {
      if (lhs.DNF == true || rhs.DNF == true)
      {
        return false;
      }

      if (lhs.Unknown == true || rhs.Unknown == true)
      {
        return false;
      }

      if (lhs.Minutes < rhs.Minutes)
      {
        return true;
      }
      else if (lhs.Minutes == rhs.Minutes && lhs.Seconds < rhs.Seconds)
      {
        return true;
      }

      return false;
    }

    /// <summary>
    /// Divide a time by and integer, return DNF if time is DNF.
    /// </summary>
    /// <param name="lhs">distance to add (left)</param>
    /// <param name="rhs">distance to add (right)</param>
    /// <returns>new distance</returns>
    public static RaceTimeType operator /(RaceTimeType lhs,
                                          int          rhs)
    {
      if (lhs.DNF)
      {
        return new RaceTimeType(true, false);
      }

      if (lhs.Unknown)
      {
        return new RaceTimeType(false, true);
      }

      int minutesInt = 0;
      int secondsInt = 0;

      double step = lhs.Minutes + (lhs.Seconds / 60.0);
      double dividedTime = step / rhs;

      try
      {
        minutesInt = (int)Math.Truncate(dividedTime);
        double seconds = (dividedTime - Math.Truncate(dividedTime)) * 60;
        secondsInt = (int)Math.Round(seconds, MidpointRounding.AwayFromZero);
      }
      catch
      {
        return new RaceTimeType(0, 0);
      }

      return new RaceTimeType(minutesInt, secondsInt);
    }

    /// <summary>
    /// Return string of format "mm:ss"
    /// </summary>
    /// <returns>new string</returns>
    public override string ToString()
    {
      if (DNF)
      {
        return dnfDescription;
      }
      else if (Unknown)
      {
        return unknownDescription;
      }
      else
      {
        return Minutes.ToString() + separator + Seconds.ToString("00");
      }
    }
  }
}