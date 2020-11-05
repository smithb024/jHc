namespace HandicapModel.Admin.IO
{
  using System.IO;

  using TXT;

  /// <summary>
  /// Static class to describe the root paths.
  /// </summary>
  public static class RootPath
  {
    /// <summary>
    /// Configuration path;
    /// </summary>
    public static string ConfigurationPath =>
      RootIO.LoadRootFile() +
      Path.DirectorySeparatorChar +
      IOPaths.configurationPath;

    /// <summary>
    /// Data path;
    /// </summary>
    public static string DataPath =>
      RootIO.LoadRootFile() +
      Path.DirectorySeparatorChar +
      IOPaths.dataPath;
  }
}