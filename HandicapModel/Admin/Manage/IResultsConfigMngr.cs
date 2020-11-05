namespace HandicapModel.Admin.Manage
{
    using CommonHandicapLib.Types;

    /// <summary>
    /// Interface which describes the results configuration manager
    /// </summary>
    public interface IResultsConfigMngr
    {
        /// <summary>
        /// Gets the results configuration details;
        /// </summary>
        ResultsConfigType ResultsConfigurationDetails { get; }

        /// <summary>
        /// Creates and saves a default results configuration file.
        /// </summary>
        void SaveDefaultResultsConfiguration(bool overrideExisting = false);

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
        void SaveResultsConfiguration(
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
          bool excludeFirstTimers);

        /// <summary>
        /// Creates and saves a default results configuration file.
        /// </summary>
        /// <param name="resultsConfig">results configuration details</param>
        void SaveResultsConfiguration(ResultsConfigType resultsConfig);
    }
}