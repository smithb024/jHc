namespace CommonHandicapLib
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  public static class HandicapConverter
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="athleteAge"></param>
    /// <returns></returns>
    public static string ConvertAgeToHandicap(string athleteAge)
    {
      int age = 0;
      int handicap = 0;

      if (int.TryParse(athleteAge, out age))
      {
        if (age < 6)
        {
          handicap = 0;
        }
        else if (age < 9)
        {
          handicap = 5;
        }
        else if (age < 12)
        {
          handicap = 8;
        }
        else if (age < 15)
        {
          handicap = 10;
        }
        else
        {
          handicap = 12;
        }
      }

      return handicap.ToString();
    }
  }
}
