namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using CommonHandicapLib.Messages;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.SeasonModel;
    using jHCVMUI.ViewModels.ViewModels;
    using jHCVMUI.ViewModels.ViewModels.Types.Clubs;
    using NynaeveLib.Commands;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// View model which supports the points table for the team trophy. 
    /// </summary>
    public class TeamTrophyPointsTableViewModel : ViewModelBase
    {
        /// <summary>
        /// The associated season model.
        /// </summary>
        private readonly ISeason model;

        /// <summary>
        /// The points table.
        /// </summary>
        private ObservableCollection<TeamTrophyPointsTableRowViewModel> pointsTable;

        /// <summary>
        /// Index of the currently selected item on the table.
        /// </summary>
        private int currentTeamTrophyPointsTableIndex;

        /// <summary>
        /// Indicates whether verbose or concise data is show.
        /// </summary>
        private bool expandedData;

        /// <summary>
        /// Initialises a new instance of the <see cref="ISeason"/> class.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        public TeamTrophyPointsTableViewModel(IModel model)
        {
            this.model = model.CurrentSeason;
            this.pointsTable = new ObservableCollection<TeamTrophyPointsTableRowViewModel>();
            this.currentTeamTrophyPointsTableIndex = 0;
            this.expandedData = false;

            this.ExpandCommand =
              new SimpleCommand(
                this.UpdateExpandedFlag);

            this.PopulateClubTable();

            CommonMessenger.Default.Register<RefreshDataPaneMessage>(
                this,
                this.Refresh);
        }

        /// <summary>
        /// Gets the command used to toggle verbose/concise data.
        /// </summary>
        public ICommand ExpandCommand { get; private set; }

        /// <summary>
        /// Gets and sets the expanded data flag.
        /// </summary>
        public bool ExpandedData
        {
            get
            {
                return this.expandedData;
            }

            set
            {
                if (this.expandedData != value)
                {
                    this.expandedData = value;
                    this.RaisePropertyChangedEvent(nameof(this.ExpandCommand));
                    this.RaisePropertyChangedEvent(nameof(this.ExpandedLabel));
                    this.RaisePropertyChangedEvent(nameof(this.ExpandedData));
                }
            }
        }

        /// <summary>
        /// Gets the expanded label value.
        /// </summary>
        public string ExpandedLabel
        {
            get
            {
                return ExpandedData ? "^" : "v";
            }
        }

        /// <summary>
        /// Gets points sets the team trophy points table.
        /// </summary>
        public ObservableCollection<TeamTrophyPointsTableRowViewModel> PointsTable
        {
            get
            {
                return this.pointsTable;
            }

            set
            {
                this.pointsTable = value;
                RaisePropertyChangedEvent(nameof(this.PointsTable));
            }
        }

        /// <summary>
        /// Gets or sets the index of the club points table
        /// </summary>
        public int SelectedPointsTableIndex
        {
            get
            {
                return currentTeamTrophyPointsTableIndex;
            }

            set
            {
                currentTeamTrophyPointsTableIndex = value;
                RaisePropertyChangedEvent(nameof(this.SelectedPointsTableIndex));
            }
        }

        /// <summary>
        /// Toggle expanded data flag.
        /// </summary>
        public void UpdateExpandedFlag()
        {
            this.ExpandedData = !this.ExpandedData;

            foreach (TeamTrophyPointsTableRowViewModel row in this.PointsTable)
            {
                row.ExpandedData = this.ExpandedData;
            }
        }


        /// <summary>
        /// Calculate and populate the club points table.
        /// </summary>
        private void PopulateClubTable()
        {
            foreach (ClubSeasonDetails clubSeasonDetail in this.model.Clubs)
            {
                TeamTrophyPointsTableRowViewModel clubPoints =
                    new TeamTrophyPointsTableRowViewModel(
                        clubSeasonDetail.Name,
                        clubSeasonDetail.TeamTrophy.TotalScore);

                foreach (ITeamTrophyEvent eventPoints in clubSeasonDetail.TeamTrophy.Events)
                {
                    TeamTrophyPointsTypeViewModel points =
                        new TeamTrophyPointsTypeViewModel(
                            eventPoints.Score,
                            eventPoints.TotalAthletePoints,
                            eventPoints.NumberOfAthletes,
                            eventPoints.Points,
                            eventPoints.Date);
                    clubPoints.AddPoints(points);
                }

                this.PointsTable.Add(clubPoints);
            }

            this.PointsTable =
                new ObservableCollection<TeamTrophyPointsTableRowViewModel>(
                    this.PointsTable.OrderByDescending(
                        order => order.TotalPoints));
        }

        /// <summary>
        /// Refresh this view model.
        /// </summary>
        /// <param name="message">refresh view model message</param>
        private void Refresh(
            RefreshDataPaneMessage message)
        {
            this.PointsTable.Clear();
            this.PopulateClubTable();
        }
    }
}