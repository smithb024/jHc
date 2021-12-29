﻿namespace HandicapModel.Admin.IO.XML
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using CommonHandicapLib;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Types;
    using CommonHandicapLib.XML.ResultsTable;
    using CommonLib.Enumerations;
    using CommonLib.Types;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using HandicapModel.SeasonModel.EventModel;
    using NynaeveLib.XML;

    /// <summary>
    /// The results table reader.
    /// </summary>
    public class ResultsTableReader : IResultsTableReader
    {
        private const string c_rootElement = "ResTbl";
        private const string c_rowElement = "Row";

        private const string keyAttribute = "key";
        private const string c_clubAttribute = "club";
        private const string extraInfoAttribute = "EI";
        private const string c_handicapAttribute = "HC";
        private const string c_nameAttribute = "name";
        private const string c_notesAttribute = "notes";
        private const string c_orderAttribute = "order";
        private const string c_pbAttribute = "PB";
        private const string c_pointsAttribute = "pts";
        private const string HarmonyPointsAttribute = "hPts";
        private const string c_raceNumberAttribute = "number";
        private const string c_runningOrderAttribute = "timePosition";
        private const string c_sexAttribute = "sx";
        private const string c_timeAttribute = "time";
        private const string c_ybAttribute = "YB";
        private const string ageGradedRatingAttribute = "AGR";

        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="ResultsTableReader"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public ResultsTableReader(IJHcLogger logger)
        {
            this.logger = logger;
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
                            entry.HarmonyPoints,
                            entry.RaceNumber,
                            entry.RunningOrder,
                            entry.Time.ToString(),
                            entry.Sex);

                    rootElement.Add(entryElement);
                }

                string fileName =
                    ResultsTableReader.GetPath(
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
                ResultsTableReader.GetPath(
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
                        row.Points,
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
                            row.HarmonyPoints,
                            row.IsPersonalBest,
                            row.IsYearBest,
                            string.Empty,
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
        private static string GetPath(
            string seasonName,
            string eventName)
        {
            return RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + IOPaths.athleteResultsTable;
        }
    }
}