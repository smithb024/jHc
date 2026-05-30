namespace CommonHandicapLib.Messages
{
    /// <summary>
    /// Message which is sent via the messenger to request that the data pane is refreshed from the
    /// model.
    /// </summary>
    public class RefreshDataPaneMessage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="RefreshDataPaneMessage"/> class.
        /// </summary>
        /// <param name="refreshMobPoints">refresh the mob trophy view model</param>
        /// <param name="refreshTeamPoints">refresh the team trophy view model</param>
        /// <param name="refreshPoints">refresh the points table view model</param>
        /// <param name="refreshEventSummary">refresh event summary view model</param>
        /// <param name="refreshTotalSummay">refresh total summary view model</param>
        /// <param name="refreshResults">refresh results table view model</param>
        public RefreshDataPaneMessage(
            bool refreshMobPoints,
            bool refreshTeamPoints,
            bool refreshPoints,
            bool refreshEventSummary,
            bool refreshTotalSummay,
            bool refreshResults) 
        {
            this.RefreshMobTrophyPointsTable = refreshMobPoints;
            this.RefreshTeamTrophyPointsTable = refreshTeamPoints;
            this.RefreshPointsTable = refreshPoints;
            this.RefreshSummaryEvent = refreshEventSummary;
            this.RefreshSummaryTotal = refreshTotalSummay;
            this.RefreshResultsTable = refreshResults;
        }

        /// <summary>
        /// Gets a value indicating whether to refresh the mob trophy points table view model.
        /// </summary>
        public bool RefreshMobTrophyPointsTable { get; }

        /// <summary>
        /// Gets a value indicating whether to refresh the team trophy points table view model.
        /// </summary>
        public bool RefreshTeamTrophyPointsTable { get; }

        /// <summary>
        /// Gets a value indicating whether to refresh the points table view model.
        /// </summary>
        public bool RefreshPointsTable { get; }

        /// <summary>
        /// Gets a value indicating whether to refresh the event summary view model.
        /// </summary>
        public bool RefreshSummaryEvent { get; }

        /// <summary>
        /// Gets a value indicating whether to refresh the total summary view model.
        /// </summary>
        public bool RefreshSummaryTotal { get; }

        /// <summary>
        /// Gets a value indicating whether to refresh the results table view model.
        /// </summary>
        public bool RefreshResultsTable { get; }
    }
}
