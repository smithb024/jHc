namespace HandicapModel.Interfaces.Admin.IO.XML
{
    using HandicapModel.Interfaces.Common;

    /// <summary>
    /// The summary data reader
    /// </summary>
    public interface ISummaryDataReader
    {
        /// <summary>
        /// Contructs the xml and writes it to a data file
        /// </summary>
        /// <param name="fileName">name of the summary file</param>
        /// <param name="summaryDetails">details to save</param>
        bool SaveSummaryData(
            string fileName,
            ISummary summaryDetails);

        /// <summary>
        /// Reads the athlete details xml from file and decodes it.
        /// </summary>
        /// <param name="fileName">name of xml file</param>
        /// <returns>decoded summary data</returns>
        ISummary ReadCompleteSummaryData(string fileName);
    }
}