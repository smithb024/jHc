using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHandicapLib.Helpers.EventRawResults
{
  using CommonLib.Converters;
  using CommonLib.Decoders;

  /// <summary>
  /// 
  /// </summary>
  public static class ResultsDecoder
  {
    private static char resultsSeparator = ',';

    private static string positionValueIndicator = "P";

    /// <summary>
    /// Determines whether the <paramref name="input"/> is a position string.
    /// </summary>
    /// <param name="input">string to check</param>
    /// <returns>flag indicating whether the value is a position</returns>
    public static bool IsPositionValue(string input)
    {
      return input.Substring(0, positionValueIndicator.Length).Equals(positionValueIndicator);
    }

    /// <summary>
    /// Strip the position indicator off the front of the input string. Return the string, if one 
    /// is not present.
    /// </summary>
    /// <param name="input">string to manipulate</param>
    /// <returns>the position number as a string</returns>
    public static string GetPositionNumber(string input)
    {
      if (!ResultsDecoder.IsPositionValue(input))
      {
        return input;
      }

      return input.Substring(positionValueIndicator.Length);
    }

    /// <summary>
    /// Converts the position string to an integer.
    /// This assumes that the string is confirmed to be a position input.
    /// </summary>
    /// <param name="input">string to convert</param>
    /// <returns>position value, null if <paramref name="input"/> is invalid</returns>
    public static int? ConvertPositionValue(string input)
    {
      return StringToIntConverter.ConvertStringToInt(input.Substring(1));
    }

    /// <summary>
    /// Take a results string read by the OPN scanner and return the interesting part.
    /// </summary>
    /// <param name="input">input string</param>
    /// <returns>barcode part</returns>
    public static string OpnScannerResultsBarcode(string input)
    {
      return SplitStringDecoder.ReturnSectionOfString(input, ',', 1);
    }

    /// <summary>
    /// Take a results string read by the OPN scanner and return the peripheral data.
    /// </summary>
    /// <param name="input">input string</param>
    /// <returns>barcode part</returns>
    public static string OpnScannerResultsOtherData(string input)
    {
      return input.Substring(input.IndexOf(resultsSeparator) + 1);
    }
  }
}
