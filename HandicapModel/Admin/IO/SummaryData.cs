namespace HandicapModel.Admin.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommonHandicapLib.Interfaces;
    using HandicapModel.Admin.IO.XML;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.Interfaces.Common;

    /// <summary>
    /// Summary data
    /// </summary>
    public class SummaryData : ISummaryData
    {
        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// The summary data reader
        /// </summary>
        ISummaryDataReader summaryDataReader;

        /// <summary>
        /// Initialises a new instance of the <see cref="SummaryData"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public SummaryData(IJHcLogger logger)
        {
            this.logger = logger;
            this.summaryDataReader =
                new SummaryDataReader(
                    this.logger);
        }

        /// <summary>
        /// Saves the summary details
        /// </summary>
        /// <param name="summaryDetails">details to save</param>
        /// <returns>success flag</returns>
        public bool SaveSummaryData(ISummary summaryDetails)
        {
            return this.summaryDataReader.SaveSummaryData(
              RootPath.DataPath + IOPaths.globalSummaryFile,
              summaryDetails);
        }

        /// <summary>
        /// Saves the summary details
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="summaryDetails">details to save</param>
        /// <returns>success flag</returns>
        public bool SaveSummaryData(
            string seasonName,
            ISummary summaryDetails)
        {
            return this.summaryDataReader.SaveSummaryData(
              RootPath.DataPath + Path.DirectorySeparatorChar + seasonName + Path.DirectorySeparatorChar + seasonName + IOPaths.xmlExtension,
              summaryDetails);
        }

        /// <summary>
        /// Saves the summary details
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">event name</param>
        /// <param name="summaryDetails">details to save</param>
        /// <returns>success flag</returns>
        public bool SaveSummaryData(
            string seasonName,
            string eventName,
            ISummary summaryDetails)
        {
            return this.summaryDataReader.SaveSummaryData(
              RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + eventName + IOPaths.xmlExtension,
              summaryDetails);
        }

        /// <summary>
        /// Reads the summary details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <returns>decoded summary data</returns>
        public ISummary LoadSummaryData()
        {
            return this.summaryDataReader.ReadCompleteSummaryData(RootPath.DataPath + IOPaths.globalSummaryFile);
        }

        /// <summary>
        /// Reads the summary details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <returns>decoded summary data</returns>
        public ISummary LoadSummaryData(string seasonName)
        {
            return this.summaryDataReader.ReadCompleteSummaryData(
              RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + seasonName + IOPaths.xmlExtension);
        }

        /// <summary>
        /// Reads the summary details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <returns>decoded summary data</returns>
        public ISummary LoadSummaryData(
            string seasonName,
            string eventName)
        {
            return this.summaryDataReader.ReadCompleteSummaryData(
              RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + eventName + IOPaths.xmlExtension);
        }
    }
}