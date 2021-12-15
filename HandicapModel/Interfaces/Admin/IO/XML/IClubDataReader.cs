namespace HandicapModel.Interfaces.Admin.IO.XML
{
    using HandicapModel.ClubsModel;

    /// <summary>
    /// The club data reader
    /// </summary>
    public interface IClubDataReader
    {
        /// <summary>
        /// Contructs the xml and writes it to a data file
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="clubList">list of clubs</param>
        bool SaveClubData(
            string fileName,
            Clubs clubList);

        /// <summary>
        /// Loads the club list from the data file and returns it.
        /// </summary>
        /// <param name="fileName">file name</param>
        Clubs LoadClubData(string fileName);
    }
}