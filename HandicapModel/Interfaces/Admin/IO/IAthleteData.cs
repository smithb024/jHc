namespace HandicapModel.Interfaces.Admin.IO
{
    using System.Collections.Generic;
    using HandicapModel.Admin.Manage;
    using HandicapModel.AthletesModel;
    using HandicapModel.SeasonModel;

    /// <summary>
    /// Athlete data interface
    /// </summary>
    public interface IAthleteData
    {
        /// <summary>
        /// Saves the athlete details list.
        /// </summary>
        /// <param name="athleteDetailsList">athletes to save</param>
        bool SaveAthleteData(Athletes athleteDetailsList);

        /// <summary>
        /// Reads the athlete details.
        /// </summary>
        /// <param name="fileName">name of xml file</param>
        /// <returns>decoded athlete's details</returns>
        Athletes ReadAthleteData();

        /// <summary>
        /// Saves the season athlete details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="seasons">season details to save</param>
        /// <returns>success flag</returns>
        bool SaveAthleteSeasonData(
            string seasonName,
            List<AthleteSeasonDetails> seasons);

        /// <summary>
        /// Loads the season athlete details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <returns>season athlete details</returns>
        List<AthleteSeasonDetails> LoadAthleteSeasonData(
          string seasonName,
          IResultsConfigMngr resultsConfigurationManager);
    }
}