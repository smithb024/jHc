using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jHCVMUI.ViewModels.Common
{
  using System.IO;
  using System.Windows.Input;

  using jHCVMUI.ViewModels.Commands.Main;

  /// <summary>
  ///  Factory class, used in conjunction with COTS software. 
  /// </summary>
  public static class OpenCotsFactory
  {
    /// <summary>
    /// constant defining executable extension.
    /// </summary>
    private const string Extension = ".lnk";

    /// <summary>
    /// Path to the COTS Apps from the this executable.W
    /// </summary>
    private const string Path = ".//Apps//";

    /// <summary>
    ///Create and return a command to run an executable in the <see cref="Path"/> folder when
    ///run.
    /// </summary>
    /// <param name="executable">name of the executable</param>
    /// <returns>command object</returns>
    public static ICommand CreateCommand(
      string executable)
    {
      // If the directory doesn't exist return a command which cannot be run.
      if (!Directory.Exists(Path))
      {
        return new RunApplicationCmd(
          string.Empty,
          false);
      }

      string[] allFiles = Directory.GetFiles(Path, "*");
      string fullPath = 
        OpenCotsFactory.GetFullPath(
          executable);
      bool exists =
        OpenCotsFactory.ExecutableExists(
          fullPath);

      ICommand command =
        new RunApplicationCmd(
          fullPath,
          exists);

      return command;
    }

    /// <summary>
    /// Determine whether there is a file in the indicated indicated path. Note the filename should 
    /// be included in the path.
    /// </summary>
    /// <param name="path">path and filename to check</param>
    /// <returns>value indicating whether the file is present in the location.</returns>
    private static bool ExecutableExists(
      string path)
    {
      return File.Exists(path);
    }

    /// <summary>
    /// Determine the full file path.
    /// </summary>
    /// <param name="executable">filename excluding extension</param>
    /// <returns>full path</returns>
    private static string GetFullPath(
      string executable)
    {
      return $"{Path}{executable}{Extension}";
    }
  }
}
