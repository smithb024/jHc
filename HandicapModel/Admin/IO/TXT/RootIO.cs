namespace HandicapModel.Admin.IO.TXT
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using CommonHandicapLib;

  public static class RootIO
  {
    public static string RootDataFile => "." + Path.DirectorySeparatorChar + IOPaths.rootDataFile;

    /// <summary>
    /// Save the parameter in the root folder.
    /// </summary>
    /// <param name="rootDirectory">root directory for all the mode information</param>
    /// <returns>successful flag</returns>
    public static bool SaveRootFile(string rootDirectory)
    {
      try
      {
        using (
          StreamWriter writer =
          new StreamWriter(
           RootDataFile,
           false))
        {
          writer.Write(rootDirectory);
          return true;
        }
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    /// <summary>
    /// Load the location of the root directory.
    /// </summary>
    /// <returns>root directory</returns>
    public static string LoadRootFile()
    {
      try
      {
        if (File.Exists(RootDataFile))
        {
          using (StreamReader reader = new StreamReader(RootDataFile))
          {
            return reader.ReadLine();
          }
        }
      }
      catch (Exception ex)
      {
        return string.Empty;
      }

      return string.Empty;
    }
  }
}