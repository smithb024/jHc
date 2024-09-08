namespace HandicapModel.Admin.IO
{
    using System.IO;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using HandicapModel.Admin.IO.TXT;
    using HandicapModel.Admin.IO.XML;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.Interfaces.Common;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

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
        private ISummaryDataReader summaryDataReader;

        /// <summary>
        /// The root directory.
        /// </summary>
        private string rootDirectory;

        /// <summary>
        /// The path to all the season data.
        /// </summary>
        private string dataPath;

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

            this.rootDirectory = RootIO.LoadRootFile();
            this.dataPath = $"{this.rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.dataPath}{Path.DirectorySeparatorChar}";

            CommonMessenger.Default.Register<ReinitialiseRoot>(this, this.ReinitialiseRoot);
        }

        /// <summary>
        /// Saves the summary details
        /// </summary>
        /// <param name="summaryDetails">details to save</param>
        /// <returns>success flag</returns>
        public bool SaveSummaryData(ISummary summaryDetails)
        {
            return this.summaryDataReader.SaveSummaryData(
              this.dataPath + IOPaths.globalSummaryFile,
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
              this.dataPath + Path.DirectorySeparatorChar + seasonName + Path.DirectorySeparatorChar + seasonName + IOPaths.xmlExtension,
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
              this.dataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + eventName + IOPaths.xmlExtension,
              summaryDetails);
        }

        /// <summary>
        /// Reads the summary details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <returns>decoded summary data</returns>
        public ISummary LoadSummaryData()
        {
            string path = $"{this.dataPath}{IOPaths.globalSummaryFile}";
            ISummary summary =
                this.summaryDataReader.ReadCompleteSummaryData(
                    path);
            return summary;
        }

        /// <summary>
        /// Reads the summary details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <returns>decoded summary data</returns>
        public ISummary LoadSummaryData(string seasonName)
        {
            return this.summaryDataReader.ReadCompleteSummaryData(
              this.dataPath + seasonName + Path.DirectorySeparatorChar + seasonName + IOPaths.xmlExtension);
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
              this.dataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + eventName + IOPaths.xmlExtension);
        }

        /// <summary>
        /// Reinitialise the data path value from the file.
        /// </summary>
        /// <param name="message">reinitialise message</param>
        private void ReinitialiseRoot(ReinitialiseRoot message)
        {
            this.rootDirectory = RootIO.LoadRootFile();
            this.dataPath = $"{this.rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.dataPath}{Path.DirectorySeparatorChar}";
        }
    }
}