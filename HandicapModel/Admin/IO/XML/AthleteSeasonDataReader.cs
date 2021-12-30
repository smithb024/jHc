namespace HandicapModel.Admin.IO.XML
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using CommonHandicapLib;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Types;
    using CommonLib.Types;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.SeasonModel;

    /// <summary>
    /// Athlete season data reader
    /// </summary>
    internal class AthleteSeasonDataReader : IAthleteSeasonDataReader
    {
        private const string c_rootElement = "AtlSea";
        private const string c_athleteElement = "entrant";
        private const string c_eventPointsElement = "pt";
        private const string c_pointsElement = "pts";
        private const string HarmonyPointsElement = "hPts";
        private const string c_runningNumbersElement = "runningNumbers";
        private const string c_timesElement = "tms";
        private const string c_numberElement = "number";
        private const string c_eventTimeElement = "time";

        private const string c_keyAttribute = "key";
        private const string c_bestPoints = "bpts";
        private const string c_eventDateAttribute = "date";
        private const string c_eventTimeAttribute = "time";
        private const string c_finishingPoints = "fpts";
        private const string c_nameAttribute = "name";
        private const string c_numberAttribute = "no";
        private const string c_positionPoints = "ppts";
        private const string HarmonyPointsAttribute = "pts";

        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="AthleteSeasonDataReader"/> class.
        /// </summary>
        /// <param name="logger">application logger</param>
        public AthleteSeasonDataReader(IJHcLogger logger)
        {
            this.logger = logger;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>SaveAthleteSeasonSata</name>
        /// <date>29/03/15</date>
        /// <summary>
        /// Save the points table
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="table">points table</param>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public bool SaveAthleteSeasonData(
            string fileName,
            List<AthleteSeasonDetails> seasons)
        {
            bool success = true;

            try
            {
                XDocument writer = new XDocument(
                 new XDeclaration("1.0", "uft-8", "yes"),
                 new XComment("Athlete's season XML"));
                XElement rootElement = new XElement(c_rootElement);

                foreach (AthleteSeasonDetails season in seasons)
                {
                    XElement athleteElement =
                        new XElement(
                            c_athleteElement,
                            new XAttribute(c_keyAttribute, season.Key.ToString()),
                            new XAttribute(c_nameAttribute, season.Name));

                    XElement runningNumberElement = new XElement(c_runningNumbersElement);
                    athleteElement.Add(runningNumberElement);
                    XElement timesElement = new XElement(c_timesElement);
                    athleteElement.Add(timesElement);
                    XElement pointsElement = new XElement(c_pointsElement);
                    athleteElement.Add(pointsElement);
                    XElement harmonyPointsElement = new XElement(HarmonyPointsElement);
                    athleteElement.Add(harmonyPointsElement);

                    foreach (Appearances time in season.Times)
                    {
                        XElement timeElement = new XElement(c_eventTimeElement,
                                                            new XAttribute(c_eventTimeAttribute, time.Time.ToString()),
                                                            new XAttribute(c_eventDateAttribute, time.Date.ToString()));
                        timesElement.Add(timeElement);
                    }

                    foreach (CommonPoints points in season.Points.AllPoints)
                    {
                        XElement pointElement = new XElement(c_eventPointsElement,
                                                             new XAttribute(c_finishingPoints, points.FinishingPoints.ToString()),
                                                             new XAttribute(c_positionPoints, points.PositionPoints.ToString()),
                                                             new XAttribute(c_bestPoints, points.BestPoints.ToString()),
                                                             new XAttribute(c_eventDateAttribute, points.Date.ToString()));
                        pointsElement.Add(pointElement);
                    }

                    foreach (IAthleteHarmonyPoints point in season.HarmonyPoints.AllPoints)
                    {
                        XElement pointElement =
                            new XElement(
                                c_eventPointsElement,
                                new XAttribute(HarmonyPointsAttribute, point.Point.ToString()),
                                new XAttribute(c_eventDateAttribute, point.Date.ToString()));

                        harmonyPointsElement.Add(pointElement);
                    }

                    rootElement.Add(athleteElement);
                }

                writer.Add(rootElement);
                writer.Save(fileName);
            }

            catch (Exception ex)
            {
                success = false;
                this.logger.WriteLog("Error writing Athlete points data " + ex.ToString());
            }

            return success;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>SaveAthleteSeasonData</name>
        /// <date>30/03/15</date>
        /// <summary>
        /// Reads the athlete season details xml from file and decodes it.
        /// </summary>
        /// <param name="fileName">name of xml file</param>
        /// <returns>decoded athlete's details</returns>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public List<AthleteSeasonDetails> LoadAthleteSeasonData(
          string fileName,
          IResultsConfigMngr resultsConfigurationManager)
        {
            List<AthleteSeasonDetails> seasonDetails = new List<AthleteSeasonDetails>();

            try
            {
                XDocument reader = XDocument.Load(fileName);
                XElement rootElement = reader.Root;

                var athleteList = from Athlete in rootElement.Elements(c_athleteElement)
                                  select new
                                  {
                                      key = (int)Athlete.Attribute(c_keyAttribute),
                                      name = (string)Athlete.Attribute(c_nameAttribute),
                                      runningNumbers = from RunningNumbers in Athlete.Elements(c_runningNumbersElement)
                                                       select new
                                                       {
                                                           numbers = from Numbers in RunningNumbers.Elements(c_numberElement)
                                                                     select new
                                                                     {
                                                                         number = (string)Numbers.Attribute(c_numberAttribute)
                                                                     }
                                                       },
                                      eventTimes = from EventTimes in Athlete.Elements(c_timesElement)
                                                   select new
                                                   {
                                                       events = from Times in EventTimes.Elements(c_eventTimeElement)
                                                                select new
                                                                {
                                                                    time = (string)Times.Attribute(c_eventTimeAttribute),
                                                                    date = (string)Times.Attribute(c_eventDateAttribute)
                                                                }
                                                   },
                                      points = from Points in Athlete.Elements(c_pointsElement)
                                               select new
                                               {
                                                   point = from Point in Points.Elements(c_eventPointsElement)
                                                           select new
                                                           {
                                                               finishing = (int)Point.Attribute(c_finishingPoints),
                                                               position = (int)Point.Attribute(c_positionPoints),
                                                               best = (int)Point.Attribute(c_bestPoints),
                                                               date = (string)Point.Attribute(c_eventDateAttribute)
                                                           }
                                               },
                                      harmonyPoints = from Points in Athlete.Elements(HarmonyPointsElement)
                                               select new
                                               {
                                                   point = from Point in Points.Elements(c_eventPointsElement)
                                                           select new
                                                           {
                                                               harmonyPoint = (int)Point.Attribute(HarmonyPointsAttribute),
                                                               date = (string)Point.Attribute(c_eventDateAttribute)
                                                           }
                                               }
                                  };

                foreach (var athlete in athleteList)
                {
                    AthleteSeasonDetails athleteDetails =
                      new AthleteSeasonDetails(
                        athlete.key,
                        athlete.name,
                        resultsConfigurationManager);

                    foreach (var eventTms in athlete.eventTimes)
                    {
                        foreach (var times in eventTms.events)
                        {
                            athleteDetails.AddNewTime(new Appearances(new RaceTimeType(times.time),
                                                                      new DateType(times.date)));
                        }
                    }

                    foreach (var points in athlete.points)
                    {
                        foreach (var point in points.point)
                        {
                            DateType eventDate =
                                new DateType(
                                    point.date);

                            CommonPoints commonPoints =
                                new CommonPoints(
                                    point.finishing,
                                    point.position,
                                    point.best,
                                    eventDate);

                            athleteDetails.Points.AddNewEvent(commonPoints);
                            // TODO, should probably check that there are the correct number read from the xml file.
                            // i.e. there is one for each event in the currently loaded season.
                            // Will want to change it to proper serialisation at some point.
                        }
                    }

                    foreach(var points in athlete.harmonyPoints)
                    {
                        foreach(var point in points.point)
                        {
                            DateType date = new DateType(point.date);
                            IAthleteHarmonyPoints newEvent =
                                new AthleteHarmonyPoints(
                                    point.harmonyPoint,
                                    date);

                            athleteDetails.HarmonyPoints.AddNewEvent(newEvent);
                        }
                    }

                    seasonDetails.Add(athleteDetails);
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error reading athlete data: " + ex.ToString());

                seasonDetails = new List<AthleteSeasonDetails>();
            }

            return seasonDetails;
        }
    }
}