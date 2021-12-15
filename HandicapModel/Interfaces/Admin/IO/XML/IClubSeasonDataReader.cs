namespace HandicapModel.Interfaces.Admin.IO.XML
{
    using System.Collections.Generic;
    using HandicapModel.Interfaces.SeasonModel;

    /// <summary>
    /// The club season data reader
    /// </summary>
    public interface IClubSeasonDataReader
    {
        /// <summary>
        /// Save the points table
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="table">points table</param>
        bool SaveClubSeasonData(
            string fileName,
            List<IClubSeasonDetails> seasons);

        /// <summary>
        /// Reads the athlete season details xml from file and decodes it.
        /// </summary>
        /// <param name="fileName">name of xml file</param>
        /// <returns>decoded athlete's details</returns>
        List<IClubSeasonDetails> LoadClubSeasonData(string fileName);
    }
}