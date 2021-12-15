namespace HandicapModel.Admin.IO
{
  using System.IO;
  using CommonHandicapLib;

  /// <summary>
  /// Provides any global io methods.
  /// </summary>
  public static class GeneralIO
  {
    /// <summary>
    /// Gets the global athlete's data file
    /// </summary>
    public static string AthletesGlobalDataFile
    {
      get { return RootPath.ConfigurationPath + Path.DirectorySeparatorChar + IOPaths.athleteDataFile; }
    }

    /// <summary>
    /// Gets the global club's data file
    /// </summary>
    public static string ClubGlobalDataFile
    {
      get { return RootPath.ConfigurationPath + Path.DirectorySeparatorChar + IOPaths.clubDataFile; }
    }

    /// <summary>
    /// Gets the results configuration file.
    /// </summary>
    public static string ResultsConfigurationFile
    {
      get { return RootPath.ConfigurationPath + Path.DirectorySeparatorChar + IOPaths.resultsConfiguration; }
    }

    /// <summary>
    /// Gets the normalisation configuration file.
    /// </summary>
    public static string NormalisationConfigurationFile
    {
      get { return RootPath.ConfigurationPath + Path.DirectorySeparatorChar + IOPaths.normalisationConfiguration; }
    }

    /// <summary>
    /// Gets the series configuration file.
    /// </summary>
    public static string SeriesConfigurationFile
    {
      get { return RootPath.ConfigurationPath + Path.DirectorySeparatorChar + IOPaths.seriesConfiguration; }
    }

    /// <summary>
    /// Gets a value which indicates whether the data folder exits.
    /// </summary>
    public static bool DataFolderExists => Directory.Exists(RootPath.DataPath);

    /// <summary>
    /// Gets a value which indicates whether the configuration folder exits.
    /// </summary>
    public static bool ConfigurationFolderExists => Directory.Exists(RootPath.ConfigurationPath);

    /// <summary>
    /// Creates the data folder if one does not exist.
    /// </summary>
    public static void CreateDataFolder()
    {
      if (!Directory.Exists(RootPath.DataPath))
      {
        JHcLogger.Instance.WriteLog("Data directory missing - new one created");
        Directory.CreateDirectory(RootPath.DataPath);
      }
    }

    /// <summary>
    /// Creates the data folder if one does not exist.
    /// </summary>
    public static void CreateConfigurationFolder()
    {
      if (!Directory.Exists(RootPath.ConfigurationPath))
      {
        JHcLogger.Instance.WriteLog("Configuration directory missing - new one created");
        Directory.CreateDirectory(RootPath.ConfigurationPath);
      }
    }
  }
}