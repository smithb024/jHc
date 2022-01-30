namespace HandicapModel.Admin.IO.XML
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using CommonHandicapLib;
    using CommonHandicapLib.Interfaces;
    using CommonLib.Types;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.SeasonModel;

    /// <summary>
    /// Club season data reader.
    /// </summary>
    internal class ClubSeasonDataReader : IClubSeasonDataReader
    {
        private const string c_rootElement = "CbSea";
        private const string c_clubElement = "club";
        private const string c_eventPointsElement = "pt";
        private const string c_pointsElement = "pts";
        private const string HarmonyPointsElement = "hPts";
        private const string EventElement = "event";

        private const string finishingPointsAttribute = "fPt";
        private const string positionPointsAttribute = "pPt";
        private const string bestPointsAttribute = "bPt";
        private const string eventDateAttribute = "evPt";
        private const string HarmonyTeamSizeAttribute = "tm";
        private const string HarmonyVirtualAthletePointAttribute = "vr";
        private const string HarmonyEventDateAttribute = "dt";
        private const string HarmonyScoreAttribute = "scr";
        private const string HarmonyPointAttribute = "pt";
        private const string AthleteKeyAttribute = "key";

        private const string nameAttribute = "name";

        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="ClubSeasonDataReader"/> class.
        /// </summary>
        /// <param name="logger">application logger</param>
        public ClubSeasonDataReader(IJHcLogger logger)
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
        public bool SaveClubSeasonData(
            string fileName,
            List<IClubSeasonDetails> seasons)
        {
            bool success = true;

            try
            {
                XDocument writer = new XDocument(
                 new XDeclaration("1.0", "uft-8", "yes"),
                 new XComment("Athlete's season XML"));
                XElement rootElement = new XElement(c_rootElement);

                foreach (ClubSeasonDetails season in seasons)
                {
                    XElement athleteElement = 
                        new XElement(
                            c_clubElement,
                            new XAttribute(nameAttribute, season.Name));

                    XElement pointsElement = new XElement(c_pointsElement);
                    athleteElement.Add(pointsElement);

                    XElement harmonyPointsElement = new XElement(HarmonyPointsElement);
                    athleteElement.Add(harmonyPointsElement);

                    foreach (CommonPoints points in season.ClubCompetition.Points)
                    {
                        XElement pointElement =
                            new XElement(
                                c_eventPointsElement,
                                new XAttribute(finishingPointsAttribute, points.FinishingPoints.ToString()),
                                new XAttribute(positionPointsAttribute, points.PositionPoints.ToString()),
                                new XAttribute(bestPointsAttribute, points.BestPoints.ToString()),
                                new XAttribute(eventDateAttribute, points.Date.ToString()));
                        pointsElement.Add(pointElement);
                    }

                    foreach(IHarmonyEvent harmonyEvent in season.HarmonyCompetition.Events)
                    {
                        XElement eventElement =
                            new XElement(
                                EventElement,
                                new XAttribute(HarmonyTeamSizeAttribute, harmonyEvent.Points.Count),
                                new XAttribute(HarmonyVirtualAthletePointAttribute, harmonyEvent.VirtualAthletePoints),
                                new XAttribute(HarmonyEventDateAttribute, harmonyEvent.Date.ToString()),
                                new XAttribute(HarmonyScoreAttribute, harmonyEvent.Score));

                        foreach(ICommonHarmonyPoints point in harmonyEvent.Points)
                        {
                            if (point.IsReal)
                            {
                                XElement points =
                                    new XElement(
                                        c_eventPointsElement,
                                        new XAttribute(HarmonyPointAttribute, point.Point),
                                        new XAttribute(AthleteKeyAttribute, point.AthleteKey));

                                eventElement.Add(points);
                            }
                        }

                        harmonyPointsElement.Add(eventElement);
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
        public List<IClubSeasonDetails> LoadClubSeasonData(string fileName)
        {
            List<IClubSeasonDetails> seasonDetails = new List<IClubSeasonDetails>();

            try
            {
                XDocument reader = XDocument.Load(fileName);
                XElement rootElement = reader.Root;

                var clubList = from Club in rootElement.Elements(c_clubElement)
                               select new
                               {
                                   name = (string)Club.Attribute(nameAttribute),
                                   points = from Points in Club.Elements(c_pointsElement)
                                            select new
                                            {
                                                point = from Point in Points.Elements(c_eventPointsElement)
                                                        select new
                                                        {
                                                            finishing = (int)Point.Attribute(finishingPointsAttribute),
                                                            position = (int)Point.Attribute(positionPointsAttribute),
                                                            best = (int)Point.Attribute(bestPointsAttribute),
                                                            date = (string)Point.Attribute(eventDateAttribute)
                                                        }
                                            },
                                   harmonyPoints = from HarmonyPoints in Club.Elements(HarmonyPointsElement)
                                                   select new
                                                   {
                                                       events = from HarmonyEvent in HarmonyPoints.Elements(EventElement)
                                                                select new
                                                                {
                                                                    size = (int)HarmonyEvent.Attribute(HarmonyTeamSizeAttribute),
                                                                    virtualPoint = (int)HarmonyEvent.Attribute(HarmonyVirtualAthletePointAttribute),
                                                                    date = (string)HarmonyEvent.Attribute(HarmonyEventDateAttribute),
                                                                    score = (int)HarmonyEvent.Attribute(HarmonyScoreAttribute),
                                                                    points = from Point in HarmonyEvent.Elements(c_eventPointsElement)
                                                                            select new
                                                                            {
                                                                                value = (int)Point.Attribute(HarmonyPointAttribute),
                                                                                key = (int)Point.Attribute(AthleteKeyAttribute)
                                                                            }
                                                                }
                                                   }
                               };

                foreach (var club in clubList)
                {
                    ClubSeasonDetails clubDetails = new ClubSeasonDetails(club.name);

                    foreach (var points in club.points)
                    {
                        foreach (var point in points.point)
                        {
                            DateType date =
                                new DateType(
                                    point.date);
                            CommonPoints readPoints =
                                new CommonPoints(
                                    point.finishing,
                                    point.position,
                                    point.best,
                                    date);


                            clubDetails.ClubCompetition.AddNewEvent(readPoints);
                            // TODO, should probably check that there are the correct number read from the xml file.
                            // i.e. there is one for each event in the currently loaded season.
                        }
                    }

                    foreach (var harmonyPoints in club.harmonyPoints)
                    {

                        foreach (var harmonyEvent in harmonyPoints.events)
                        {
                            List<ICommonHarmonyPoints> pointsList = new List<ICommonHarmonyPoints>();
                            DateType date =
                                new DateType(
                                    harmonyEvent.date);

                            foreach (var point in harmonyEvent.points)
                            {
                                CommonHarmonyPoints readPoints =
                                    new CommonHarmonyPoints(
                                        point.value,
                                        string.Empty,
                                        point.key,
                                        true,
                                        date);

                                pointsList.Add(readPoints);
                            }

                            IHarmonyEvent readEvent =
                                new HarmonyEvent(
                                    date,
                                    pointsList,
                                    harmonyEvent.size,
                                    harmonyEvent.score);
                            readEvent.Complete(
                                harmonyEvent.size,
                                harmonyEvent.virtualPoint);

                            clubDetails.HarmonyCompetition.AddEvent(readEvent);
                        }
                    }


                    seasonDetails.Add(clubDetails);
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error reading athlete data: " + ex.ToString());

                seasonDetails = new List<IClubSeasonDetails>();
            }

            return seasonDetails;
        }
    }
}