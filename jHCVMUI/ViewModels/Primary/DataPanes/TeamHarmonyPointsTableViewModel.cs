namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.SeasonModel;
    using jHCVMUI.ViewModels.ViewModels;
    using jHCVMUI.ViewModels.ViewModels.Types.Clubs;
    using NynaeveLib.Commands;

    /// <summary>
    /// View model which supports the points table for the team (harmony) trophy. This is the
    /// trophy which behaves like a mob match with a maximum team size.
    /// </summary>
    public class TeamHarmonyPointsTableViewModel : ViewModelBase
    {
        /// <summary>
        /// The points table.
        /// </summary>
        private ObservableCollection<TeamHarmonyPointsTableRowViewModel> pointsTable;

        /// <summary>
        /// Index of the currently selected item on the table.
        /// </summary>
        private int currentTeamHarmonyPointsTableIndex;

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
        public TeamHarmonyPointsTableViewModel(ISeason model)
        {
            this.model = model;
            this.pointsTable = new ObservableCollection<TeamHarmonyPointsTableRowViewModel>();
            this.currentTeamHarmonyPointsTableIndex = 0;
            this.expandedData = false;

            //Model.Instance.CurrentSeason.ClubsCallback =
            //    new ClubSeasonDelegate(
            //        PopulateClubPointsData);

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

        public string Test => "My Test";

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
        /// Gets points sets the team (harmony) points table.
        /// </summary>
        public ObservableCollection<TeamHarmonyPointsTableRowViewModel> PointsTable
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
                return currentTeamHarmonyPointsTableIndex;
            }

            set
            {
                currentTeamHarmonyPointsTableIndex = value;
                RaisePropertyChangedEvent(nameof(this.SelectedPointsTableIndex));
            }
        }

        /// <summary>
        /// Toggle expanded data flag.
        /// </summary>
        public void UpdateExpandedFlag()
        {
            this.ExpandedData = !this.ExpandedData;

            foreach (TeamHarmonyPointsTableRowViewModel row in this.PointsTable)
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
                TeamHarmonyPointsTableRowViewModel clubPoints =
                    new TeamHarmonyPointsTableRowViewModel(
                        clubSeasonDetail.Name,
                        clubSeasonDetail.HarmonyCompetition.TotalScore);

                //ClubPointsTableRowViewModel clubPoints =
                //    new ClubPointsTableRowViewModel(
                //        clubSeasonDetail.Name,
                //        clubSeasonDetail.ClubCompetition.TotalPoints,
                //        clubSeasonDetail.ClubCompetition.TotalFinishingPoints.ToString(),
                //        clubSeasonDetail.ClubCompetition.TotalPositionPoints.ToString(),
                //        clubSeasonDetail.ClubCompetition.TotalBestPoints.ToString());

                foreach (IHarmonyEvent eventPoints in clubSeasonDetail.HarmonyCompetition.Events)
                {
                    HarmonyPointsTypeViewModel points =
                        new HarmonyPointsTypeViewModel(
                            eventPoints.TotalAthletePoints,
                            eventPoints.NumberOfAthletes,
                            eventPoints.Points,
                            eventPoints.Date);
                    clubPoints.Points.Add(points);
                }
                //foreach (CommonPoints eventPoints in clubSeasonDetail.ClubCompetition.Points)
                //{
                //    clubPoints.AddPoints(
                //        new PointsType(
                //            eventPoints.FinishingPoints,
                //            eventPoints.PositionPoints,
                //            eventPoints.BestPoints,
                //            eventPoints.Date));
                //}

                PointsTable.Add(clubPoints);
            }

            this.PointsTable =
                new ObservableCollection<TeamHarmonyPointsTableRowViewModel>(
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