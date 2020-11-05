namespace CommonHandicapLib.Helpers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using CommonLib.Types;
  using Types;

  public static class HandicapHelper
  {
    /// <summary>
    /// Round to the nearest 30 seconds
    /// </summary>
    /// <param name="config">normalisation configuration</param>
    /// <param name="inputTime">time to round</param>
    /// <returns></returns>
    public static TimeType RoundHandicap(
      NormalisationConfigType config,
      TimeType inputTime)
    {
      TimeType calculatedHandicap = inputTime;

      // Make sure the handicap is not less than the minimum.
      if (calculatedHandicap < new TimeType(config.MinimumHandicap, 0))
      {
        return new TimeType(config.MinimumHandicap, 0);
      }

      // Round to nearest 30 seconds
      if (calculatedHandicap.Seconds < 15)
      {
        calculatedHandicap = new TimeType(calculatedHandicap.Minutes, 0);
      }
      else if (calculatedHandicap.Seconds < 45)
      {
        calculatedHandicap = new TimeType(calculatedHandicap.Minutes, 30);
      }
      else
      {
        calculatedHandicap = new TimeType(calculatedHandicap.Minutes + 1, 0);
      }

      return calculatedHandicap;
    }

    /// <summary>
    /// Round to the nearest 30 seconds
    /// </summary>
    /// <param name="config">normalisation configuration</param>
    /// <param name="inputTime">time to round</param>
    /// <returns></returns>
    public static RaceTimeType RoundHandicap(
      NormalisationConfigType config,
      RaceTimeType inputTime)
    {
      RaceTimeType calculatedHandicap = inputTime;

      // Make sure the handicap is not less than the minimum.
      if (calculatedHandicap < new TimeType(config.MinimumHandicap, 0))
      {
        return new RaceTimeType(config.MinimumHandicap, 0);
      }

      // Round to nearest 30 seconds
      if (calculatedHandicap.Seconds < 15)
      {
        calculatedHandicap = new RaceTimeType(calculatedHandicap.Minutes, 0);
      }
      else if (calculatedHandicap.Seconds < 45)
      {
        calculatedHandicap = new RaceTimeType(calculatedHandicap.Minutes, 30);
      }
      else
      {
        calculatedHandicap = new RaceTimeType(calculatedHandicap.Minutes + 1, 0);
      }

      return calculatedHandicap;
    }
  }
}