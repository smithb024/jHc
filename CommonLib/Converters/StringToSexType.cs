namespace CommonLib.Converters
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using CommonLib.Enumerations;

  public static class StringToSexType
  {
    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>convertStringToInt</name>
    /// <date>19/01/15</date>
    /// <summary>
    ///   Converts the incoming string to an integer.
    /// </summary>
    /// <param name="inputString">input string</param>
    /// <returns>integer number</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public static SexType ConvertStringToSexType(string inputString)
    {
      if (inputString == SexType.Male.ToString())
      {
        return SexType.Male;
      }
      else if (inputString == SexType.Female.ToString())
      {
        return SexType.Female;
      }
      else
      {
        return SexType.Default;
      }
    }
  }
}
