namespace jHCVMUI.ViewModels.Primary
{
    using System.Windows.Input;

    using DataPanes;
    using HandicapModel;
    using HandicapModel.Interfaces;
    using jHCVMUI.ViewModels.ViewModels;
    using NynaeveLib.Commands;

    /// <summary>
    /// View model for the Data Pane view. This view controls the part of the main window which 
    /// contains all the data.
    /// </summary>
    public class DataPaneViewModel : ViewModelBase
    {
        /// <summary>
        /// Junior handicap model.
        /// </summary>
        private IModel model;

        /// <summary>
        /// Object used to hold the main contents of the data view pane.
        /// </summary>
        private object dataViewContents;

        /// <summary>
        /// View model which supports the Mob Trophy points table.
        /// </summary>
        private MobTrophyPointsTableViewModel mobTrophyPointsTableViewModel;

        /// <summary>
        /// View model which supports the team trophy points table.
        /// </summary>
        private TeamTrophyPointsTableViewModel teamTrophyPointsTableViewModel;

        /// <summary>
        /// View model which supports the athlete points table.
        /// </summary>
        private PointsTableViewModel pointsTableViewModel;

        /// <summary>
        /// View model which supports the summary table for the current event.
        /// </summary>
        private SummaryEventViewModel eventSummaryViewModel;

        /// <summary>
        /// View model which supports the summary table for the current season.
        /// </summary>
        private SummaryTotalViewModel seasonSummaryViewModel;

        /// <summary>
        /// View model which supports the results table for the current event.
        /// </summary>
        private ResultsTableViewModel resultsTableViewModel;

        /// <summary>
        /// View model which supports the data pane on the main window.
        /// </summary>
        /// <param name="model">Junior handicap model</param>
        public DataPaneViewModel(IModel model)
        {
            this.model = model;

            this.mobTrophyPointsTableViewModel =
                new MobTrophyPointsTableViewModel(
                    this.model.CurrentSeason);
            this.teamTrophyPointsTableViewModel =
                new TeamTrophyPointsTableViewModel(
                    this.model.CurrentSeason);
            this.pointsTableViewModel = 
                new PointsTableViewModel(
                    this.model);
            this.eventSummaryViewModel = 
                new SummaryEventViewModel(
                    this.model.CurrentEvent);
            this.seasonSummaryViewModel = 
                new SummaryTotalViewModel(
                    this.model.CurrentSeason);
            this.resultsTableViewModel =
                new ResultsTableViewModel(
                    this.model.CurrentEvent);

            this.dataViewContents = this.seasonSummaryViewModel;

            this.ShowMobTrophyPointsTableCommand = new SimpleCommand(this.SelectMobTrophyPointsTable);
            this.ShowTeamTrophyPointsTableCommand = new SimpleCommand(this.SelectTeamTrophyPointsTable);
            this.ShowEventSummaryCommand = new SimpleCommand(this.SelectEventSummaryData);
            this.ShowPointsTableCommand = new SimpleCommand(this.SelectPointsTable);
            this.ShowResultsCommand = new SimpleCommand(this.SelectResultsTable);
            this.ShowSeasonSummaryCommand = new SimpleCommand(this.SelectSeasonSummaryData);
        }

        /// <summary>
        /// Gets the command used to display the results data.
        /// </summary>
        public ICommand ShowResultsCommand { get; private set; }

        /// <summary>
        /// Gets the command used to show the points table.
        /// </summary>
        public ICommand ShowPointsTableCommand { get; private set; }

        /// <summary>
        /// Gets the command used to show the Mob Trophy points table.
        /// </summary>
        public ICommand ShowMobTrophyPointsTableCommand { get; private set; }

        /// <summary>
        /// Gets the command used to show the Team Trophy points table.
        /// </summary>
        public ICommand ShowTeamTrophyPointsTableCommand { get; private set; }

        /// <summary>
        /// Gets the command used to show the event summary data.
        /// </summary>
        public ICommand ShowEventSummaryCommand { get; private set; }

        /// <summary>
        /// Gets the command used to show the season summary data.
        /// </summary>
        public ICommand ShowSeasonSummaryCommand { get; private set; }

        /// <summary>
        /// Gets or sets an object which presents the primary set of informaiton on the data view.
        /// </summary>
        public object DataViewContents
        {
            get
            {
                return this.dataViewContents;
            }

            set
            {
                this.dataViewContents = value;
                this.RaisePropertyChangedEvent(nameof(this.DataViewContents));
                this.RaisePropertyChangedEvent(nameof(this.SeasonSummaryVisible));
                this.RaisePropertyChangedEvent(nameof(this.PointsTableVisible));
                this.RaisePropertyChangedEvent(nameof(this.EventSummaryVisible));
                this.RaisePropertyChangedEvent(nameof(this.MobTrophyPointsTableVisible));
                this.RaisePropertyChangedEvent(nameof(this.TeamTrophyPointsTableVisible));
                this.RaisePropertyChangedEvent(nameof(this.ResultsTableVisible));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the season summary data is visible.
        /// </summary>
        public bool SeasonSummaryVisible => this.DataViewContents.GetType() == typeof(SummaryTotalViewModel);

        /// <summary>
        /// Gets a value indicating whether the season summary data is visible.
        /// </summary>
        public bool EventSummaryVisible => this.DataViewContents.GetType() == typeof(SummaryEventViewModel);

        /// <summary>
        /// Gets a value indicating whether the season summary data is visible.
        /// </summary>
        public bool PointsTableVisible => this.DataViewContents.GetType() == typeof(PointsTableViewModel);

        /// <summary>
        /// Gets a value indicating whether the Mob Trophy table is visible.
        /// </summary>
        public bool MobTrophyPointsTableVisible => this.DataViewContents.GetType() == typeof(MobTrophyPointsTableViewModel);

        /// <summary>
        /// Gets a value indicating whether the Team Trophy table is visible.
        /// </summary>
        public bool TeamTrophyPointsTableVisible => this.DataViewContents.GetType() == typeof(TeamTrophyPointsTableViewModel);

        /// <summary>
        /// Gets a value indicating whether the Results table is visible.
        /// </summary>
        public bool ResultsTableVisible => this.DataViewContents.GetType() == typeof(ResultsTableViewModel);

        /// <summary>
        /// Gets the date of the current event.
        /// </summary>
        public string EventDate =>
            this.model.CurrentEvent.Date.ToString();

        /// <summary>
        /// Select the event summary data for display on the data pane.
        /// </summary>
        private void SelectEventSummaryData()
        {
            this.DataViewContents = this.eventSummaryViewModel;
        }

        /// <summary>
        /// Select the season summary data for display on the data pane.
        /// </summary>
        private void SelectSeasonSummaryData()
        {
            this.DataViewContents = this.seasonSummaryViewModel;
        }

        /// <summary>
        /// Select the points table for display on the data pane.
        /// </summary>
        private void SelectPointsTable()
        {
            this.DataViewContents = this.pointsTableViewModel;
        }

        /// <summary>
        /// Select the Mob Trophy points table for display on the data pane.
        /// </summary>
        private void SelectMobTrophyPointsTable()
        {
            this.DataViewContents = this.mobTrophyPointsTableViewModel;
        }

        /// <summary>
        /// Select the Team Trophy points table for display on the data pane.
        /// </summary>
        private void SelectTeamTrophyPointsTable()
        {
            this.DataViewContents = this.teamTrophyPointsTableViewModel;
        }

        /// <summary>
        /// Select the results table for display on the data pane.
        /// </summary>
        private void SelectResultsTable()
        {
            this.DataViewContents = this.resultsTableViewModel;
        }
    }
}