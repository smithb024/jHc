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
        private const string MobTrophyPointsElement = "pts";
        private const string TeamTrophyPointsElement = "hPts";
        private const string EventElement = "event";

        private const string finishingPointsAttribute = "fPt";
        private const string positionPointsAttribute = "pPt";
        private const string bestPointsAttribute = "bPt";
        private const string eventDateAttribute = "evPt";
        private const string TeamTrophyTeamSizeAttribute = "tm";
        private const string TeamTrophyVirtualAthletePointAttribute = "vr";
        private const string TeamTrophyEventDateAttribute = "dt";
        private const string TeamTrophyScoreAttribute = "scr";
        private const string TeamTrophyPointAttribute = "pt";
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

                    XElement mobTophyPointsElement = new XElement(MobTrophyPointsElement);
                    athleteElement.Add(mobTophyPointsElement);

                    XElement teamTrophyPointsElement = new XElement(TeamTrophyPointsElement);
                    athleteElement.Add(teamTrophyPointsElement);

                    foreach (CommonPoints points in season.MobTrophy.Points)
                    {
                        XElement pointElement =
                            new XElement(
                                c_eventPointsElement,
                                new XAttribute(finishingPointsAttribute, points.FinishingPoints.ToString()),
                                new XAttribute(positionPointsAttribute, points.PositionPoints.ToString()),
                                new XAttribute(bestPointsAttribute, points.BestPoints.ToString()),
                                new XAttribute(eventDateAttribute, points.Date.ToString()));
                        mobTophyPointsElement.Add(pointElement);
                    }

                    foreach(ITeamTrophyEvent teamTrophyEvent in season.TeamTrophy.Events)
                    {
                        XElement eventElement =
                            new XElement(
                                EventElement,
                                new XAttribute(TeamTrophyTeamSizeAttribute, teamTrophyEvent.Points.Count),
                                new XAttribute(TeamTrophyVirtualAthletePointAttribute, teamTrophyEvent.VirtualAthletePoints),
                                new XAttribute(TeamTrophyEventDateAttribute, teamTrophyEvent.Date.ToString()),
                                new XAttribute(TeamTrophyScoreAttribute, teamTrophyEvent.Score));

                        foreach(ICommonTeamTrophyPoints point in teamTrophyEvent.Points)
                        {
                            if (point.IsReal)
                            {
                                XElement points =
                                    new XElement(
                                        c_eventPointsElement,
                                        new XAttribute(TeamTrophyPointAttribute, point.Point),
                                        new XAttribute(AthleteKeyAttribute, point.AthleteKey));

                                eventElement.Add(points);
                            }
                        }

                        teamTrophyPointsElement.Add(eventElement);
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
                                   mobTrophyPoints = from MobTrophyPoints in Club.Elements(MobTrophyPointsElement)
                                            select new
                                            {
                                                point = from Point in MobTrophyPoints.Elements(c_eventPointsElement)
                                                        select new
                                                        {
                                                            finishing = (int)Point.Attribute(finishingPointsAttribute),
                                                            position = (int)Point.Attribute(positionPointsAttribute),
                                                            best = (int)Point.Attribute(bestPointsAttribute),
                                                            date = (string)Point.Attribute(eventDateAttribute)
                                                        }
                                            },
                                   teamTrophyPoints = from TeamTrophyPoints in Club.Elements(TeamTrophyPointsElement)
                                                   select new
                                                   {
                                                       events = from TeamTrophyEvent in TeamTrophyPoints.Elements(EventElement)
                                                                select new
                                                                {
                                                                    size = (int)TeamTrophyEvent.Attribute(TeamTrophyTeamSizeAttribute),
                                                                    virtualPoint = (int)TeamTrophyEvent.Attribute(TeamTrophyVirtualAthletePointAttribute),
                                                                    date = (string)TeamTrophyEvent.Attribute(TeamTrophyEventDateAttribute),
                                                                    score = (int)TeamTrophyEvent.Attribute(TeamTrophyScoreAttribute),
                                                                    points = from Point in TeamTrophyEvent.Elements(c_eventPointsElement)
                                                                            select new
                                                                            {
                                                                                value = (int)Point.Attribute(TeamTrophyPointAttribute),
                                                                                key = (int)Point.Attribute(AthleteKeyAttribute)
                                                                            }
                                                                }
                                                   }
                               };

                foreach (var club in clubList)
                {
                    ClubSeasonDetails clubDetails = new ClubSeasonDetails(club.name);

                    foreach (var points in club.mobTrophyPoints)
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


                            clubDetails.MobTrophy.AddNewEvent(readPoints);
                            // TODO, should probably check that there are the correct number read from the xml file.
                            // i.e. there is one for each event in the currently loaded season.
                        }
                    }

                    foreach (var teamTrophyPoints in club.teamTrophyPoints)
                    {

                        foreach (var teamTrophyEvent in teamTrophyPoints.events)
                        {
                            List<ICommonTeamTrophyPoints> pointsList = new List<ICommonTeamTrophyPoints>();
                            DateType date =
                                new DateType(
                                    teamTrophyEvent.date);

                            foreach (var point in teamTrophyEvent.points)
                            {
                                CommonTeamTrophyPoints readPoints =
                                    new CommonTeamTrophyPoints(
                                        point.value,
                                        string.Empty,
                                        point.key,
                                        true,
                                        date);

                                pointsList.Add(readPoints);
                            }

                            ITeamTrophyEvent readEvent =
                                new TeamTrophyEvent(
                                    date,
                                    pointsList,
                                    teamTrophyEvent.size,
                                    teamTrophyEvent.score);
                            readEvent.Complete(
                                teamTrophyEvent.size,
                                teamTrophyEvent.virtualPoint);

                            clubDetails.TeamTrophy.AddEvent(readEvent);
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