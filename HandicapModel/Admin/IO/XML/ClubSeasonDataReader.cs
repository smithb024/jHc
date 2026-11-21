namespace HandicapModel.Admin.IO.XML
{
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.XML.ClubSeasonData;
    using CommonLib.Types;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.SeasonModel;
    using NynaeveLib.XML;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// Club season data reader.
    /// </summary>
    internal class ClubSeasonDataReader : IClubSeasonDataReader
    {
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
                CbSeaRoot root = new CbSeaRoot();

                foreach (IClubSeasonDetails club in seasons)
                {
                    Club translatedClub =
                        new Club
                        {
                            Name = club.Name
                        };

                    MobTrophyPointsRoot mobTrophyPointsRoot = new MobTrophyPointsRoot();
                    MobTrophyPoints translatedMobPoints = new MobTrophyPoints();

                    foreach (ICommonPoints mobPoints in club.MobTrophy.Points)
                    {
                        MobTrophyPoint translatedPoint = new MobTrophyPoint();
                        translatedPoint.YbPoints = mobPoints.BestPoints;
                        translatedPoint.PositionPoints = mobPoints.PositionPoints;
                        translatedPoint.FinishingPoints = mobPoints.FinishingPoints;
                        translatedPoint.Date = mobPoints.Date.ToString();
                        translatedMobPoints.Add(translatedPoint);
                    }

                    mobTrophyPointsRoot.Points = translatedMobPoints;
                    translatedClub.MobPoints = mobTrophyPointsRoot;

                    TeamTrophyPointsRoot teamTrophyPointsRoot = new TeamTrophyPointsRoot();
                    TeamTrophyEventsRoot translatedEventRoot = new TeamTrophyEventsRoot();

                    foreach (ITeamTrophyEvent teamEvent in club.TeamTrophy.Events)
                    {
                        CommonHandicapLib.XML.ClubSeasonData.TeamTrophyEvent translatedPoint =
                            new CommonHandicapLib.XML.ClubSeasonData.TeamTrophyEvent
                            {
                                TeamSize = teamEvent.TeamSize,
                                VirtualRunnerScore = teamEvent.VirtualAthletePoints,
                                Date = teamEvent.Date.ToString(),
                                Score = teamEvent.Score
                            };

                        TeamTrophyPoints translatedTeamTrophyPoints = new TeamTrophyPoints();

                        foreach(ICommonTeamTrophyPoints teamTrophyPoint in teamEvent.Points)
                        {
                            TeamTrophyPoint translatedTeamTropyPoint =
                                new TeamTrophyPoint()
                                {
                                    Key = teamTrophyPoint.AthleteKey,
                                    Points = teamTrophyPoint.Point
                                };
                            translatedTeamTrophyPoints.Add(translatedTeamTropyPoint);
                        }

                        translatedPoint.Points = translatedTeamTrophyPoints;
                        translatedEventRoot.Add(translatedPoint);
                    }

                    teamTrophyPointsRoot.Events = translatedEventRoot;
                    translatedClub.TeamPoints = teamTrophyPointsRoot;

                    root.Add(translatedClub);
                }

                XmlFileIo.WriteXml<CbSeaRoot>(
                root,
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
        public List<IClubSeasonDetails> LoadClubSeasonData(string fileName)
        {
            List<IClubSeasonDetails> seasonDetails = new List<IClubSeasonDetails>();

            // If the file doesn't exist, create a blank one before proceeding.
            if (!File.Exists(fileName))
            {
                string error =
                    string.Format(
                        "Club data file missing, one created - {0}",
                        fileName);

                CommonMessenger.Default.Send(
                    new HandicapErrorMessage(
                        error));
                this.logger.WriteLog(error);

                this.SaveClubSeasonData(
                    fileName,
                    new List<IClubSeasonDetails>());
            }

            // Load and translate.
            try
            {
                CbSeaRoot deserialisationClubDetails =
                    XmlFileIo.ReadXml<CbSeaRoot>(
                        fileName);

                foreach (Club club in deserialisationClubDetails)
                {
                    IClubSeasonDetails translatedClub =
                        new ClubSeasonDetails(
                            club.Name);

                    foreach (MobTrophyPoint mobTrophyPoint in club.MobPoints.Points)
                    {
                        DateType date =
                                new DateType(
                                    mobTrophyPoint.Date);

                        CommonPoints readPoints =
                                new CommonPoints(
                                    mobTrophyPoint.FinishingPoints,
                                    mobTrophyPoint.PositionPoints,
                                    mobTrophyPoint.YbPoints,
                                    date);

                        translatedClub.MobTrophy.AddNewEvent(readPoints);
                    }

                    foreach (CommonHandicapLib.XML.ClubSeasonData.TeamTrophyEvent teamTrophyEvent in club.TeamPoints.Events)
                    {
                        DateType date =
                            new DateType(
                                teamTrophyEvent.Date);

                        List<ICommonTeamTrophyPoints> points = new List<ICommonTeamTrophyPoints>();

                        foreach(TeamTrophyPoint moreStuff in teamTrophyEvent.Points)
                        {
                            ICommonTeamTrophyPoints readPoint = 
                                new CommonTeamTrophyPoints(
                                      moreStuff.Points,
                                      string.Empty,
                                      moreStuff.Key,
                                      true,
                                      date);

                            points.Add(readPoint);
                        }

                        ITeamTrophyEvent readTeamTrophyEvent =
                            new SeasonModel.TeamTrophyEvent(
                                date,
                                points,
                                teamTrophyEvent.TeamSize,
                                teamTrophyEvent.VirtualRunnerScore,
                                teamTrophyEvent.Score);

                        translatedClub.TeamTrophy.AddEvent(readTeamTrophyEvent);
                    }

                    seasonDetails.Add(translatedClub);
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