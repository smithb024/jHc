namespace HandicapModel.Admin.IO.XML
{
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Types;
    using CommonHandicapLib.XML.AthleteDataSeason;
    using CommonLib.Types;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.SeasonModel;
    using NynaeveLib.XML;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Athlete season data reader
    /// </summary>
    internal class AthleteSeasonDataReader : IAthleteSeasonDataReader
    {
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
            List<IAthleteSeasonDetails> seasons)
        {
            bool success = true;

            try
            {
                AtlSeaRoot saveCollection = new AtlSeaRoot();

                foreach (IAthleteSeasonDetails season in seasons)
                {
                    EntrantTimesRoot entrantTimes = new EntrantTimesRoot();
                    entrantTimes.Appearances = new EntrantTimes();
                    foreach (Appearances appearance in season.Times)
                    {
                        EntrantTime time = 
                            new EntrantTime()
                            {
                                Time = appearance.TimeString,
                                Date = appearance.DateString
                            };
                        entrantTimes.Appearances.Add(time);
                    }

                    TeamTrophyPointsRoot entrantTeamPoints = new TeamTrophyPointsRoot();
                    entrantTeamPoints.Points = new TeamTrophyPoints();
                    foreach (IAthleteTeamTrophyPoints point in season.TeamTrophyPoints.AllPoints)
                    {
                        TeamTrophyPoint teamPoint =
                            new TeamTrophyPoint()
                            {
                                Date = point.Date.ToString(),
                                Points = point.Point
                            };
                        entrantTeamPoints.Points.Add(teamPoint);
                    }

                    MobTrophyPointsRoot entrantMobPoints = new MobTrophyPointsRoot();
                    entrantMobPoints.Points = new MobTrophyPoints();
                    foreach (CommonPoints point in season.Points.AllPoints)
                    {
                        MobTrophyPoint mobPoint =
                            new MobTrophyPoint()
                            {
                                Date = point.Date.ToString(),
                                PositionPoints = point.PositionPoints,
                                FinishingPoints = point.FinishingPoints,
                                YbPoints = point.BestPoints
                            };
                        entrantMobPoints.Points.Add(mobPoint);
                    }

                    Entrant entrant = 
                        new Entrant()
                        {
                            Key = season.Key,
                            Name = season.Name,
                            Times = entrantTimes,
                            TeamPoints = entrantTeamPoints,
                            MobPoints = entrantMobPoints
                        };

                    saveCollection.Add(entrant);
                }

                XmlFileIo.WriteXml<AtlSeaRoot>(
                    saveCollection,
                    fileName);
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
        public List<IAthleteSeasonDetails> LoadAthleteSeasonData(
          string fileName,
          IResultsConfigMngr resultsConfigurationManager)
        {
            AtlSeaRoot deserialisationAthleteSeasonDetails;

            try
            {
                deserialisationAthleteSeasonDetails =
                    XmlFileIo.ReadXml<AtlSeaRoot>(
                        fileName);
            }
            catch (XmlException ex)
            {
                this.logger.WriteLog(
                    $"Error reading the Athletes Data file: {ex.XmlMessage}");
                return new List<IAthleteSeasonDetails>();
            }

            List<IAthleteSeasonDetails> seasonDetails = new List<IAthleteSeasonDetails>();

            foreach (Entrant athlete in deserialisationAthleteSeasonDetails)
            {
                AthleteSeasonDetails athleteDetails =
                  new AthleteSeasonDetails(
                    athlete.Key,
                    athlete.Name);

                foreach (EntrantTime eventTms in athlete.Times.Appearances)
                {
                    athleteDetails.AddNewTime(
                        new Appearances(
                            new RaceTimeType(
                                eventTms.Time),
                            new DateType(
                                eventTms.Date)));
                }

                foreach (MobTrophyPoint point in athlete.MobPoints.Points)
                {
                    DateType eventDate =
                        new DateType(
                            point.Date);

                    CommonPoints commonPoints =
                        new CommonPoints(
                            point.FinishingPoints,
                            point.PositionPoints,
                            point.YbPoints,
                            eventDate);

                    athleteDetails.Points.AddNewEvent(commonPoints);
                    // TODO, should probably check that there are the correct number read from the xml file.
                    // i.e. there is one for each event in the currently loaded season.
                }

                foreach (TeamTrophyPoint point in athlete.TeamPoints.Points)
                {
                    DateType date =
                    new DateType(
                        point.Date);
                    IAthleteTeamTrophyPoints newEvent =
                        new AthleteTeamTrophyPoints(
                            point.Points,
                            date);

                    athleteDetails.TeamTrophyPoints.AddNewEvent(newEvent);
                }

                seasonDetails.Add(athleteDetails);
            }

            return seasonDetails;
        }
    }
}