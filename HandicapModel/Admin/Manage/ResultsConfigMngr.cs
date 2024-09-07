namespace HandicapModel.Admin.Manage
{
    using System;
    using System.IO;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Interfaces.XML;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using CommonHandicapLib.XML;
    using HandicapModel.Interfaces.Admin.IO;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// Results configuration manager.
    /// </summary>
    public class ResultsConfigMngr : IResultsConfigMngr
    {
        /// <summary>
        /// the configuration reader.
        /// </summary>
        private readonly IResultsConfigReader configurationReader;

        /// <summary>
        /// Application logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// The general IO manager.
        /// </summary>
        private readonly IGeneralIo generalIo;

        /// <summary>
        /// Initialise a new instance of the <see cref="ResultsConfigMngr"/> class.
        /// </summary>
        /// <param name="generalIo">general IO manager</param>
        /// <param name="logger">application model</param>
        public ResultsConfigMngr(
            IGeneralIo generalIo,
            IJHcLogger logger)
        {
            this.logger = logger;
            this.generalIo = generalIo;
            this.configurationReader =
                new ResultsConfigReader(
                    logger);

            this.ResultsConfigurationDetails =
              this.configurationReader.LoadResultsConfigData(
                this.generalIo.ResultsConfigurationFile);

            CommonMessenger.Default.Register<LoadNewSeriesMessage>(this, this.LoadNewSeries);
        }

        /// <summary>
        /// Gets the results configuration details;
        /// </summary>
        public ResultsConfigType ResultsConfigurationDetails { get; private set; }

        /// <summary>
        /// Creates and saves a default results configuration file.
        /// </summary>
        public void SaveDefaultResultsConfiguration(
          bool overrideExisting = false)
        {
            if (!File.Exists(this.generalIo.ResultsConfigurationFile) || overrideExisting)
            {
                string teamTrophyPoints = "6,5,4,3,2,1";

                this.SaveResultsConfiguration(
                  new ResultsConfigType(4, 2, 10, 4, 5, 2, 0, true, true, true, false, 8, teamTrophyPoints));
                this.logger.WriteLog("Couldn't find results config file. Created a new default one");
            }
        }

        /// <summary>
        /// Creates and saves a default results configuration file.
        /// </summary>
        /// <param name="finishingPoints">points for finishing</param>
        /// <param name="seasonBestPoints">points for running a season best</param>
        /// <param name="scoringPositions">number of positional scoring places</param>
        /// <param name="teamFinishingPoints">team points for finishing</param>
        /// <param name="teamSize">size of a team</param>
        /// <param name="teamSeasonBestPoints">team points for running a season best</param>
        /// <param name="scoresToCount">
        /// Number of scores in a season to count towards the final points table
        /// </param>
        /// <param name="allResults">
        /// A value which indicates whether all the results in a season counts towards
        /// the final points table.
        /// </param>
        /// <param name="useTeams">
        /// A value which indicates whether teams are to be used in the competition.
        /// </param>
        /// <param name="scoresAreDescending">
        /// A value which indicates whether the athelete scores are decending. I.e. 1st is high 
        /// and decends to zero. Highest score at the end of the season wins.
        /// Ascending is 1pt for 1st, 2pts for 2nd. Lowest score at the end of the season wins.
        /// </param>
        /// <param name="excludeFirstTimers">
        /// A value which indicates whether first timers should be excluded from scoring position 
        /// points.
        /// </param>
        /// <param name="teamTrophyTeamSize">
        /// The number of members of a Team Trophy team.
        /// </param>
        /// <param name="teamTrophyPointsScoring">
        /// A comma separated list detailing the points scored per position in the Team Trophy. 
        /// </param>
        public void SaveResultsConfiguration(
          int finishingPoints,
          int seasonBestPoints,
          int scoringPositions,
          int teamFinishingPoints,
          int teamSize,
          int teamSeasonBestPoints,
          int scoresToCount,
          bool allResults,
          bool useTeams,
          bool scoresAreDescending,
          bool excludeFirstTimers,
          int teamTrophyTeamSize,
          string teamTrophyPointsScoring)
        {
            this.SaveResultsConfiguration(
                new ResultsConfigType(
                  finishingPoints,
                  seasonBestPoints,
                  scoringPositions,
                  teamFinishingPoints,
                  teamSize,
                  teamSeasonBestPoints,
                  scoresToCount,
                  allResults,
                  useTeams,
                  scoresAreDescending,
                  excludeFirstTimers,
                  teamTrophyTeamSize,
                  teamTrophyPointsScoring));
        }

        /// <summary>
        /// Creates and saves a default results configuration file.
        /// </summary>
        /// <param name="resultsConfig">results configuration details</param>
        public void SaveResultsConfiguration(ResultsConfigType resultsConfig)
        {
            try
            {
                this.configurationReader.SaveResultsConfigData(
                  this.generalIo.ResultsConfigurationFile,
                  resultsConfig);
                this.ResultsConfigurationDetails = resultsConfig;
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Failed to save results config file: " + ex.ToString());
                CommonMessenger.Default.Send(
                    new HandicapErrorMessage(
                        "Error creating results config file"));
            }
        }

        /// <summary>
        /// Load a new series into the model.
        /// </summary>
        /// <param name="message">load a new series</param>
        private void LoadNewSeries(LoadNewSeriesMessage message)
        {
            this.ResultsConfigurationDetails =
                this.configurationReader.LoadResultsConfigData(
                    this.generalIo.ResultsConfigurationFile);
        }
    }
}