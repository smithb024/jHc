namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.SeasonModel;
    using jHCVMUI.ViewModels.ViewModels;
    using jHCVMUI.ViewModels.ViewModels.Types.Clubs;
    using NynaeveLib.Commands;

    /// <summary>
    /// View model which supports the points table for the team trophy. 
    /// </summary>
    public class TeamTrophyPointsTableViewModel : ViewModelBase
    {
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
        /// The associated season model.
        /// </summary>
        private ISeason model;

        /// <summary>
        /// Initialises a new instance of the <see cref="ISeason"/> class.
        /// </summary>
        /// <param name="model">season model</param>
        public TeamTrophyPointsTableViewModel(ISeason model)
        {
            this.model = model;
            this.pointsTable = new ObservableCollection<TeamTrophyPointsTableRowViewModel>();
            this.currentTeamTrophyPointsTableIndex = 0;
            this.expandedData = false;

            this.model.ClubsChangedEvent += this.PopulateClubPointsData;
            this.ExpandCommand =
              new SimpleCommand(
                this.UpdateExpandedFlag);

            this.PopulateClubTable();
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
                    clubPoints.Points.Add(points);
                }

                PointsTable.Add(clubPoints);
            }

            this.PointsTable =
                new ObservableCollection<TeamTrophyPointsTableRowViewModel>(
                    this.PointsTable.OrderBy(
                        order => order.TotalPoints));
        }

        /// <summary>
        /// Used to populate the points table
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        public void PopulateClubPointsData(
            object sender,
            EventArgs e)
        {
            this.PointsTable.Clear();
            this.PopulateClubTable();
        }
        }
}