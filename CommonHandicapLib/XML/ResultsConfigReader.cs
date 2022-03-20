namespace CommonHandicapLib.XML
{
    using System;
    using System.Xml.Linq;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Interfaces.XML;
    using CommonHandicapLib.Types;

    /// <summary>
    /// Reads and writes the results configuration data to a file.
    /// </summary>
    public class ResultsConfigReader : IResultsConfigReader
    {
        private const string rootElement = "ResultsConfiguration";
        private const string racePointsElement = "RacePoints";
        private const string mobTrophyPointsElement = "ClubPoints";
        private const string teamTrophyPointsElement = "HmClubPoints";

        private const string finishingPointsAttribute = "Finishing";
        private const string seasonBestAttribute = "SB";
        private const string scoringPositionAttribute = "Positions";
        private const string teamFinishingPointsAttribute = "TFinishing";
        private const string useTeamsAttribute = "Use";
        private const string scoresToCountAttribute = "ToCount";
        private const string allResultsAttribute = "All";
        private const string scoresDescendingAttribute = "Descending";
        private const string excludeFirstTimersAttribute = "ExclFT";

        private const string teamSizeAttribute = "NumberCount";
        private const string teamSeasonBestAttribute = "SB";

        private const string teamTrophyTeamSizeAttribute = "NumberCount";
        private const string teamTrophyScoringAttribute = "Scr";

        /// <summary>
        /// The application logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Initialise a new instance of the <see cref="ResultsConfigReader"/> class.
        /// </summary>
        /// <param name="logger">application logger</param>
        public ResultsConfigReader(
            IJHcLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Save the configuration data.
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="configData">configuration data</param>
        /// <returns>success flag</returns>
        public bool SaveResultsConfigData(
          string fileName,
          ResultsConfigType configData)
        {
            bool success = true;

            try
            {
                XDocument writer = new XDocument(
                 new XDeclaration("1.0", "uft-8", "yes"),
                 new XComment("Results Configuration XML"));
                XElement root = new XElement(rootElement);

                XElement racePoints =
                  new XElement(
                    racePointsElement,
                    new XAttribute(finishingPointsAttribute, configData.FinishingPoints),
                    new XAttribute(seasonBestAttribute, configData.SeasonBestPoints),
                    new XAttribute(scoringPositionAttribute, configData.NumberOfScoringPositions),
                    new XAttribute(excludeFirstTimersAttribute, configData.ExcludeFirstTimers),
                    new XAttribute(allResultsAttribute, configData.AllResults),
                    new XAttribute(scoresToCountAttribute, configData.ScoresToCount),
                    new XAttribute(scoresDescendingAttribute, configData.ScoresAreDescending));
                XElement mobTrophyPoints =
                  new XElement(
                    mobTrophyPointsElement,
                    new XAttribute(useTeamsAttribute, configData.UseTeams),
                    new XAttribute(teamFinishingPointsAttribute, configData.TeamFinishingPoints),
                    new XAttribute(teamSizeAttribute, configData.NumberInTeam),
                    new XAttribute(teamSeasonBestAttribute, configData.TeamSeasonBestPoints));
                XElement teamTrophyPoints =
                  new XElement(
                    teamTrophyPointsElement,
                    new XAttribute(teamTrophyTeamSizeAttribute, configData.NumberInTeamTrophyTeam),
                    new XAttribute(teamTrophyScoringAttribute, configData.TeamTrophyPointsScoring));

                root.Add(racePoints);
                root.Add(mobTrophyPoints);
                root.Add(teamTrophyPoints);

                writer.Add(root);
                writer.Save(fileName);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error writing Results Configuration data " + ex.ToString());
            }

            return success;
        }

        /// <summary>
        /// Gets the results configuration data
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>configuration data</returns>
        public ResultsConfigType LoadResultsConfigData(string fileName)
        {
            try
            {
                XDocument reader = XDocument.Load(fileName);
                XElement root = reader.Root;

                XElement RacePoints = root.Element(racePointsElement);
                XElement MobTrophyPoints = root.Element(mobTrophyPointsElement);
                XElement TeamTrophyPoints = root.Element(teamTrophyPointsElement);

                int finishingPoints = (int)RacePoints.Attribute(finishingPointsAttribute);
                int seasonBestPoints = (int)RacePoints.Attribute(seasonBestAttribute);
                int scoringPositions = (int)RacePoints.Attribute(scoringPositionAttribute);
                bool excludeFirstTimers =
                  this.ReadBoolAttribute(
                    RacePoints,
                    excludeFirstTimersAttribute,
                    true,
                    fileName);
                bool allResults =
                  this.ReadBoolAttribute(
                    RacePoints,
                    allResultsAttribute,
                    true,
                    fileName);
                int scoresToCount =
                  this.ReadIntAttribute(
                    RacePoints,
                    scoresToCountAttribute,
                    0);
                bool scoresAreDescending =
                  this.ReadBoolAttribute(
                    RacePoints,
                    scoresDescendingAttribute,
                    true,
                    fileName);

                bool useTeams =
                  this.ReadBoolAttribute(
                    MobTrophyPoints,
                    useTeamsAttribute,
                    true,
                    fileName);
                int teamFinishingPoints = (int)MobTrophyPoints.Attribute(teamFinishingPointsAttribute);
                int mobTrophyTeamSize = (int)MobTrophyPoints.Attribute(teamSizeAttribute);
                int teamSeasonBestPoints = (int)MobTrophyPoints.Attribute(teamSeasonBestAttribute);
                
                int teamTrophyTeamSize;
                string teamTrophyScoring;

                if (TeamTrophyPoints != null)
                {
                     teamTrophyTeamSize = (int)TeamTrophyPoints.Attribute(teamTrophyTeamSizeAttribute);
                     teamTrophyScoring = (string)TeamTrophyPoints.Attribute(teamTrophyScoringAttribute);
                }
                else
                {
                    teamTrophyTeamSize = 0;
                    teamTrophyScoring = string.Empty;
                }

                return new ResultsConfigType(
                  finishingPoints,
                  seasonBestPoints,
                  scoringPositions,
                  teamFinishingPoints,
                  mobTrophyTeamSize,
                  teamSeasonBestPoints,
                  scoresToCount,
                  allResults,
                  useTeams,
                  scoresAreDescending,
                  excludeFirstTimers,
                  teamTrophyTeamSize,
                  teamTrophyScoring);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error reading Results Configuration data " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Read an integer attribute. Log and return a default value if there is an issue.
        /// </summary>
        /// <param name="element">element to read</param>
        /// <param name="attributeName">attribute name</param>
        /// <param name="defaultValue">default value</param>
        /// <param name="path">path to the file being read</param>
        /// <returns>attribute value</returns>
        private bool ReadBoolAttribute(
          XElement element,
          string attributeName,
          bool defaultValue,
          string path)
        {
            // TODO, could be generic. There are others
            try
            {
                return (bool)element.Attribute(attributeName);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog(
                  $"Error reading Results Configuration data attribute: {attributeName} : {path} :{ex}");
                return defaultValue;
            }
        }

        /// <summary>
        /// Read an integer attribute. Log and return a default value if there is an issue.
        /// </summary>
        /// <param name="element">element to read</param>
        /// <param name="attributeName">attribute name</param>
        /// <param name="defaultValue">default value</param>
        /// <returns>attribute value</returns>
        private int ReadIntAttribute(
          XElement element,
          string attributeName,
          int defaultValue)
        {
            try
            {
                return (int)element.Attribute(attributeName);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog(
                  $"Error reading Normalisation Configuration data attribute: {ex}");
                return defaultValue;
            }
        }
    }
}