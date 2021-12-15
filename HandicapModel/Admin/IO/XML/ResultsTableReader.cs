namespace HandicapModel.Admin.IO.XML
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
    using CommonLib.Enumerations;
    using CommonLib.Types;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using HandicapModel.SeasonModel.EventModel;

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

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>SaveAthleteSeasonSata</name>
        /// <date>29/03/15</date>
        /// <summary>
        /// Save the points table
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">event name</param>
        /// <param name="resultsTable">points table</param>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public bool SaveResultsTable(
            string seasonName,
            string eventName,
            List<IResultsTableEntry> resultsTable)
        {
            bool success = true;

            try
            {
                XDocument writer = new XDocument(
                 new XDeclaration("1.0", "uft-8", "yes"),
                 new XComment("Athlete's season XML"));
                XElement rootElement = new XElement(c_rootElement);

                foreach (ResultsTableEntry entry in resultsTable)
                {
                    XElement entryElement =
                        new XElement(
                            c_rowElement,
                            new XAttribute(keyAttribute, entry.Key),
                            new XAttribute(c_nameAttribute, entry.Name),
                            new XAttribute(c_clubAttribute, entry.Club),
                            new XAttribute(c_handicapAttribute, entry.Handicap.ToString()),
                            new XAttribute(c_notesAttribute, entry.Notes),
                            new XAttribute(extraInfoAttribute, entry.ExtraInfo),
                            new XAttribute(c_orderAttribute, entry.Order),
                            new XAttribute(c_pbAttribute, entry.PB.ToString()),
                            new XAttribute(c_pointsAttribute, entry.Points.ToString()),
                            new XAttribute(HarmonyPointsAttribute, entry.HarmonyPoints.ToString()),
                            new XAttribute(c_raceNumberAttribute, entry.RaceNumber.ToString()),
                            new XAttribute(c_runningOrderAttribute, entry.RunningOrder.ToString()),
                            new XAttribute(c_timeAttribute, entry.Time.ToString()),
                            new XAttribute(c_sexAttribute, entry.Sex.ToString()),
                            new XAttribute(c_ybAttribute, entry.SB),
                            new XAttribute(ageGradedRatingAttribute, entry.AgeGrading));

                    rootElement.Add(entryElement);
                }

                writer.Add(rootElement);
                writer.Save(RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + IOPaths.athleteResultsTable);
            }

            catch (Exception ex)
            {
                this.logger.WriteLog("Error writing results table" + ex.ToString());
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
            string resultsPath = RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + IOPaths.athleteResultsTable;

            try
            {
                if (File.Exists(resultsPath))
                {
                    XDocument reader = XDocument.Load(resultsPath);
                    XElement rootElement = reader.Root;

                    var rowList = from Row in rootElement.Elements(c_rowElement)
                                  select new
                                  {
                                      key = (int)Row.Attribute(keyAttribute),
                                      club = (string)Row.Attribute(c_clubAttribute),
                                      hc = (string)Row.Attribute(c_handicapAttribute),
                                      name = (string)Row.Attribute(c_nameAttribute),
                                      notes = (string)Row.Attribute(c_notesAttribute),
                                      extraInfo = (string)Row.Attribute(extraInfoAttribute),
                                      order = (int)Row.Attribute(c_orderAttribute),
                                      pb = (bool)Row.Attribute(c_pbAttribute),
                                      points = (string)Row.Attribute(c_pointsAttribute),
                                      harmonyPoints = (int)Row.Attribute(HarmonyPointsAttribute),
                                      number = (string)Row.Attribute(c_raceNumberAttribute),
                                      runningOrder = (int)Row.Attribute(c_runningOrderAttribute),
                                      time = (string)Row.Attribute(c_timeAttribute),
                                      sex = (string)Row.Attribute(c_sexAttribute),
                                      yb = (bool)Row.Attribute(c_ybAttribute),
                                      ageGradedRating = (string)Row.Attribute(ageGradedRatingAttribute)
                                  };

                    foreach (var row in rowList)
                    {
                        SexType sex = SexType.Default;
                        if (!Enum.TryParse(row.sex, out sex))
                        {
                            this.logger.WriteLog("Error reading sex from " + row.name);
                        }

                        ResultsTableEntry rowEntry =
                          new ResultsTableEntry(
                            row.key,
                            row.name,
                            new RaceTimeType(row.time),
                            row.order,
                            row.runningOrder,
                            new RaceTimeType(row.hc),
                            row.club,
                            sex,
                            row.number,
                            new CommonPoints(row.points, date),
                            row.harmonyPoints,
                            row.pb,
                            row.yb,
                            row.ageGradedRating,
                            row.notes,
                            row.extraInfo,
                            resultsTable.Entries.Count + 1);
                        resultsTable.AddEntry(rowEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error reading results table: " + ex.ToString());

                resultsTable = new EventResults();
            }

            return resultsTable;
        }
    }
}