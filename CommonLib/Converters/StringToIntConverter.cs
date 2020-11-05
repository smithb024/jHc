namespace CommonLib.Converters
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public static class StringToIntConverter
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
    public static int? ConvertStringToInt(string inputString)
    {
      int tempNumber = 0;

      if (!int.TryParse(inputString, out tempNumber))
      {
        //TODO what if can't convert. Don't want a call to the log here.
        //Logger logger = Logger.GetInstance();
        //logger.WriteLog("ERROR: Failed to convert number from string to int: " + inputString);
      }

      return tempNumber;
    }
  }
}
