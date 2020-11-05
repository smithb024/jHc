using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Helpers
{
  /// <summary>
  /// Static class, used to manage conversion from a collection to a single string.
  /// </summary>
  public static class StringCollectionOutput
  {
    /// <summary>
    /// Convert the <paramref name="inputCollection"/> to a comma separated list.
    /// </summary>
    /// <typeparam name="T">Type of the input collection</typeparam>
    /// <param name="inputCollection">collection to convert</param>
    /// <returns>comma separated string.</returns>
    public static string ListToString<T>(IList<T> inputCollection)
    {
      // Ensure the collection is valid.
      if (inputCollection == null || inputCollection.Count == 0)
      {
        return string.Empty;
      }

      // Convert valid collection.
      string returnString = string.Empty;

      for (int index = 0; index < inputCollection.Count; ++index)
      {
        if (index == 0)
        {
          returnString = inputCollection[index].ToString();
        }
        else
        {
          returnString += $", {inputCollection[index]}";
        }
      }

      return returnString;
    }
  }
}