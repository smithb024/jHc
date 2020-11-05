namespace HandicapModel.Admin.IO
{
  using System.Collections.Generic;
  using System.IO;
  using HandicapModel.Admin.IO.XML;
  using HandicapModel.Admin.Manage;
  using HandicapModel.AthletesModel;
  using HandicapModel.SeasonModel;

  public static class AthleteData
  {
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>SaveAthleteData</name>
    /// <date>22/02/15</date>
    /// <summary>
    /// Saves the athlete details list.
    /// </summary>
    /// <param name="athleteDetailsList">athletes to save</param>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static bool SaveAthleteData(Athletes athleteDetailsList)
    {
      return AthleteDataReader.SaveAthleteData(GeneralIO.AthletesGlobalDataFile, athleteDetailsList);
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>ReadAthleteData</name>
    /// <date>22/02/15</date>
    /// <summary>
    /// Reads the athlete details.
    /// </summary>
    /// <param name="fileName">name of xml file</param>
    /// <returns>decoded athlete's details</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static Athletes ReadAthleteData()
    {
      return AthleteDataReader.ReadAthleteData(GeneralIO.AthletesGlobalDataFile);
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>SaveAthleteSeasonData</name>
    /// <date>03/04/15</date>
    /// <summary>
    /// Saves the season athlete details.
    /// </summary>
    /// <param name="seasonName">season name</param>
    /// <param name="seasons">season details to save</param>
    /// <returns>success flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static bool SaveAthleteSeasonData(string                     seasonName,
                                             List<AthleteSeasonDetails> seasons)
    {
      return AthleteSeasonDataReader.SaveAthleteSeasonData(
        RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.athleteDataFile,
        seasons);
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>LoadAthleteSeasonData</name>
    /// <date>03/04/15</date>
    /// <summary>
    /// Loads the season athlete details.
    /// </summary>
    /// <param name="seasonName">season name</param>
    /// <returns>season athlete details</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static List<AthleteSeasonDetails> LoadAthleteSeasonData(
      string seasonName,
      IResultsConfigMngr resultsConfigurationManager)
    {
      return AthleteSeasonDataReader.LoadAthleteSeasonData(
        RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.athleteDataFile,
        resultsConfigurationManager);
    }
  }
}
