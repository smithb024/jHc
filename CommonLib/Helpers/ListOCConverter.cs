namespace CommonLib.Helpers
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;

  /// <summary>
  /// List to Observable Collection Converter.
  /// </summary>
  public static class ListOCConverter
  {
    /// <summary>
    /// Covert a list of objects to an observableCollection of objects
    /// </summary>
    /// <typeparam name="T">type of the lists</typeparam>
    /// <param name="originList">original list</param>
    /// <returns>observable collection</returns>
    public static ObservableCollection<T> ToObservableCollection<T>(List<T> originList)
    {
      ObservableCollection<T> returnCollection = new ObservableCollection<T>();

      foreach (T listObject in originList)
      {
        returnCollection.Add(listObject);
      }

      return returnCollection;
    }

    /// <summary>
    /// Covert a list of objects to an observableCollection of objects
    /// </summary>
    /// <typeparam name="T">type of the lists</typeparam>
    /// <param name="originCollection">original collection</param>
    /// <returns>new list</returns>
    public static List<T> ToList<T>(ObservableCollection<T> originCollection)
    {
      List<T> returnList= new List<T>();

      foreach (T collectionObject in originCollection)
      {
        returnList.Add(collectionObject);
      }

      return returnList;
    }
  }
}