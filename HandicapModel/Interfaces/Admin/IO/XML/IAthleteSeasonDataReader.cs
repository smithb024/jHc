namespace HandicapModel.Interfaces.Admin.IO.XML
{
    using System.Collections.Generic;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Interfaces.SeasonModel;

    /// <summary>
    /// The athlete season data reader
    /// </summary>
    public interface IAthleteSeasonDataReader
    {
        /// <summary>
        /// Save the points table
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="table">points table</param>
        bool SaveAthleteSeasonData(
            string fileName,
            List<IAthleteSeasonDetails> seasons);

        /// <summary>
        /// Reads the athlete season details xml from file and decodes it.
        /// </summary>
        /// <param name="fileName">name of xml file</param>
        /// <returns>decoded athlete's details</returns>
        List<IAthleteSeasonDetails> LoadAthleteSeasonData(
            string fileName,
            IResultsConfigMngr resultsConfigurationManager);
    }
}