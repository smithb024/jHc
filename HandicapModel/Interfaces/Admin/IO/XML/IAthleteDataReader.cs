namespace HandicapModel.Interfaces.Admin.IO.XML
{
    using HandicapModel.AthletesModel;

    /// <summary>
    /// The athlete data reader
    /// </summary>
    public interface IAthleteDataReader
    {
        /// <summary>
        /// Contructs the xml and writes it to a data file
        /// </summary>
        bool SaveAthleteData(
            string fileName,
            Athletes athleteDetailsList);

        /// <summary>
        /// Reads the athlete details xml from file and decodes it.
        /// </summary>
        /// <param name="fileName">name of xml file</param>
        /// <returns>decoded athlete's details</returns>
        Athletes ReadAthleteData(string fileName);
    }
}