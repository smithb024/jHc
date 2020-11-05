namespace HandicapModel.Admin.IO
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using HandicapModel.Admin.IO.XML;
  using HandicapModel.ClubsModel;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.SeasonModel;

  public static class ClubData
  {
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>SaveClubData</name>
    /// <date>22/02/15</date>
    /// <summary>
    /// Saves the club list
    /// </summary>
    /// <param name="fileName">file name</param>
    /// <param name="clubList">list of clubs</param>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static bool SaveClubData(Clubs clubList)
    {
      return ClubDataReader.SaveClubData(GeneralIO.ClubGlobalDataFile, clubList);
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>LoadClubData</name>
    /// <date>22/02/15</date>
    /// <summary>
    /// Loads the club list from the data file and returns it.
    /// </summary>
    /// <param name="fileName">file name</param>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static Clubs LoadClubData()
    {
      return ClubDataReader.LoadClubData(GeneralIO.ClubGlobalDataFile);
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <summary>
    /// Saves the season club details.
    /// </summary>
    /// <param name="seasonName">season name</param>
    /// <param name="seasons">season details to save</param>
    /// <returns>success flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static bool SaveClubSeasonData(string                  seasonName,
                                          List<IClubSeasonDetails> seasons)
    {
      return ClubSeasonDataReader.SaveClubSeasonData(
        RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.clubDataFile,
        seasons);
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <summary>
    /// Loads the season club details.
    /// </summary>
    /// <param name="seasonName">season name</param>
    /// <returns>season club details</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static List<IClubSeasonDetails> LoadClubSeasonData(string seasonName)
    {
      return ClubSeasonDataReader.LoadClubSeasonData(
        RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.clubDataFile);
    }
  }
}
