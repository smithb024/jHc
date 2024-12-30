namespace HandicapModel.Admin.IO.XML
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Xml.Linq;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using CommonHandicapLib.XML.ResultsTable;
    using CommonLib.Types;
    using HandicapModel.Admin.IO.TXT;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using HandicapModel.SeasonModel.EventModel;
    using NynaeveLib.XML;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// The results table reader.
    /// </summary>
    public class ResultsTableReader : IResultsTableReader
    {
        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// The root directory.
        /// </summary>
        private string rootDirectory;

        /// <summary>
        /// The path to all the season data.
        /// </summary>
        private string dataPath;

        /// <summary>
        /// Initialises a new instance of the <see cref="ResultsTableReader"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public ResultsTableReader(IJHcLogger logger)
        {
            this.logger = logger;

            this.rootDirectory = RootIO.LoadRootFile();
            this.dataPath = $"{this.rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.dataPath}{Path.DirectorySeparatorChar}";

            CommonMessenger.Default.Register<ReinitialiseRoot>(this, this.ReinitialiseRoot);
        }

        /// <summary>
        /// Save the points table
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">event name</param>
        /// <param name="resultsTable">points table</param>
        public bool SaveResultsTable(
            string seasonName,
            string eventName,
            List<IResultsTableEntry> resultsTable)
        {
            bool success = true;

            try
            {
                ResultsTableRoot rootElement = new ResultsTableRoot();

                XDocument writer = new XDocument(
                 new XDeclaration("1.0", "uft-8", "yes"),
                 new XComment("Results Table XML"));

                foreach (ResultsTableEntry entry in resultsTable)
                {
                    Row entryElement =
                        new Row(
                            entry.Key,
                            entry.Name,
                            entry.Club,
                            entry.Handicap.ToString(),
                            entry.Notes,
                            entry.ExtraInfo,
                            entry.Order,
                            entry.PB,
                            entry.SB,
                            entry.Points.ToString(),
                            entry.TeamTrophyPoints,
                            entry.RaceNumber,
                            entry.RunningOrder,
                            entry.Time.ToString(),
                            entry.Sex);

                    rootElement.Add(entryElement);
                }

                string fileName =
                    this.GetPath(
                        seasonName,
                        eventName);
                XmlFileIo.WriteXml<ResultsTableRoot>(
                        rootElement,
                        fileName);
            }

            catch (XmlException ex)
            {
                this.logger.WriteLog($"Error writing results table file: {ex.XmlMessage}");
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Reads the athlete season details xml from file and decodes it.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">event name</param>
        /// <param name="date">event date</param>
        /// <returns>decoded athlete's details</returns>
        public IEventResults LoadResultsTable(
            string seasonName,
            string eventName,
            DateType date)
        {
            IEventResults resultsTable = new EventResults();
            ResultsTableRoot deserialisedResultTable;
            string resultsPath =
                this.GetPath(
                    seasonName,
                    eventName);

            try
            {
                deserialisedResultTable =
                    XmlFileIo.ReadXml<ResultsTableRoot>(
                        resultsPath);
            }
            catch (XmlException ex)
            {
                this.logger.WriteLog(
                    $"Error reading the results table; {ex.XmlMessage}");

                return resultsTable;
            }

            foreach(Row row in deserialisedResultTable)
            {
                RaceTimeType time =
                    new RaceTimeType(
                        row.Time);
                RaceTimeType handicap =
                    new RaceTimeType(
                        row.Handicap);
                CommonPoints points =
                    new CommonPoints(
                        row.MobTrophyPoints,
                        date);
                int position = resultsTable.Entries.Count + 1;

                ResultsTableEntry rowEntry =
                          new ResultsTableEntry(
                            row.Key,
                            row.Name,
                            time,
                            row.Order,
                            row.RunningOrder,
                            handicap,
                            row.Club,
                            row.Sex,
                            row.Number,
                            points,
                            row.TeamTrophyPoints,
                            row.IsPersonalBest,
                            row.IsYearBest,
                            row.Notes,
                            row.ExtraInformation,
                            position);
                resultsTable.AddEntry(rowEntry);
            }

            return resultsTable;
        }

        /// <summary>
        /// Create and return the file path.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">event name</param>
        /// <returns>XML path</returns>
        private string GetPath(
            string seasonName,
            string eventName)
        {
            return this.dataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + IOPaths.athleteResultsTable;
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