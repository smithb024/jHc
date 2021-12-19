namespace CommonHandicapLib.Interfaces.XML
{
  using Types;

  public interface IResultsConfigReader
  {
    /// <summary>
    /// Save the configuration data.
    /// </summary>
    /// <param name="fileName">file name</param>
    /// <param name="configData">configuration data</param>
    /// <returns>success flag</returns>
    bool SaveResultsConfigData(
      string fileName,
      ResultsConfigType configData);

    /// <summary>
    /// Gets the results configuration data
    /// </summary>
    /// <param name="fileName">file name</param>
    /// <returns>configuration data</returns>
    ResultsConfigType LoadResultsConfigData(string fileName);
  }
}
