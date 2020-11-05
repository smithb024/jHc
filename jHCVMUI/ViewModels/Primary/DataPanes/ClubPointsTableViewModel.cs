namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using HandicapModel;
    using HandicapModel.Admin.Manage;
    using HandicapModel.SeasonModel;
    using HandicapModel.Common;
    using jHCVMUI.ViewModels.ViewModels;
    using jHCVMUI.ViewModels.ViewModels.Types;
    using jHCVMUI.ViewModels.ViewModels.Types.Clubs;

    using NynaeveLib.Commands;
    using HandicapModel.Interfaces;
    using System;
    using HandicapModel.Interfaces.SeasonModel;

    /// <summary>
    /// View Model used by the Data Pane to display the club points table.
    /// </summary>
    public class ClubPointsTableViewModel : ViewModelBase
    {
        /// <summary>
        /// The club points table.
        /// </summary>
        private ObservableCollection<ClubPointsTableRowViewModel> clubPointsTable;

        /// <summary>
        /// Index of the currently selected item on the table.
        /// </summary>
        private int currentClubPointsTableIndex;

        /// <summary>
        /// Indicates whether verbose or concise data is show.
        /// </summary>
        private bool expandedData;

        /// <summary>
        /// The associated season model.
        /// </summary>
        private ISeason model;

        /// <summary>
        /// View model which suports the club points table.
        /// </summary>
        /// <param name="model">season model</param>
        public ClubPointsTableViewModel(ISeason model)
        {
            this.model = model;
            this.clubPointsTable = new ObservableCollection<ClubPointsTableRowViewModel>();
            this.currentClubPointsTableIndex = 0;
            this.expandedData = false;

            this.model.ClubsChangedEvent += this.PopulateClubPointsData;
            this.PopulateClubTable();

            this.ExpandCommand =
              new SimpleCommand(
                this.UpdateExpandedFlag);
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
                return expandedData;
            }

            set
            {
                expandedData = value;
                RaisePropertyChangedEvent("ExpandedData");
                RaisePropertyChangedEvent("ExpandedLabel");
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
        /// Gets points sets the club points table.
        /// </summary>
        public ObservableCollection<ClubPointsTableRowViewModel> ClubPointsTable
        {
            get
            {
                return clubPointsTable;
            }

            set
            {
                clubPointsTable = value;
                RaisePropertyChangedEvent("ClubPointsTable");
            }
        }

        /// <summary>
        /// Gets or sets the index of the club points table
        /// </summary>
        public int SelectedClubPointsTableIndex
        {
            get
            {
                return currentClubPointsTableIndex;
            }

            set
            {
                currentClubPointsTableIndex = value;
                RaisePropertyChangedEvent("SelectedClubPointsTableIndex");
            }
        }

        /// <summary>
        /// Toggle expanded data flag.
        /// </summary>
        public void UpdateExpandedFlag()
        {
            ExpandedData = !ExpandedData;

            foreach (ClubPointsTableRowViewModel clubPoints in ClubPointsTable)
            {
                clubPoints.ExpandedData = ExpandedData;
            }
        }

        /// <summary>
        /// Used to populate the club points table
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        public void PopulateClubPointsData(
            object sender,
            EventArgs e)
        {
            ClubPointsTable.Clear();
            this.PopulateClubTable();

            //foreach (ClubSeasonDetails clubSeasonDetail in this.model.Clubs)
            //{
            //    ClubPointsTableRowViewModel clubPoints =
            //        new ClubPointsTableRowViewModel(
            //            clubSeasonDetail.Name,
            //            clubSeasonDetail.ClubCompetition.TotalPoints,
            //            clubSeasonDetail.ClubCompetition.TotalFinishingPoints.ToString(),
            //            clubSeasonDetail.ClubCompetition.TotalPositionPoints.ToString(),
            //            clubSeasonDetail.ClubCompetition.TotalBestPoints.ToString());

            //    foreach (CommonPoints eventPoints in clubSeasonDetail.ClubCompetition.Points)
            //    {
            //        clubPoints.AddPoints(
            //            new PointsType(
            //                eventPoints.FinishingPoints,
            //                eventPoints.PositionPoints,
            //                eventPoints.BestPoints,
            //                eventPoints.Date));
            //    }

            //    ClubPointsTable.Add(clubPoints);
            //}

            //ClubPointsTable =
            //    new ObservableCollection<ClubPointsTableRowViewModel>(
            //        ClubPointsTable.OrderByDescending(
            //            order => order.TotalPoints));
        }

        /// <summary>
        /// Calculate and populate the club points table.
        /// </summary>
        private void PopulateClubTable()
        {
            foreach (ClubSeasonDetails clubSeasonDetail in this.model.Clubs)
            {
                ClubPointsTableRowViewModel clubPoints =
                    new ClubPointsTableRowViewModel(
                        clubSeasonDetail.Name,
                        clubSeasonDetail.ClubCompetition.TotalPoints,
                        clubSeasonDetail.ClubCompetition.TotalFinishingPoints.ToString(),
                        clubSeasonDetail.ClubCompetition.TotalPositionPoints.ToString(),
                        clubSeasonDetail.ClubCompetition.TotalBestPoints.ToString());

                foreach (CommonPoints eventPoints in clubSeasonDetail.ClubCompetition.Points)
                {
                    clubPoints.AddPoints(
                        new PointsType(
                            eventPoints.FinishingPoints,
                            eventPoints.PositionPoints,
                            eventPoints.BestPoints,
                            eventPoints.Date));
                }

                ClubPointsTable.Add(clubPoints);
            }

            ClubPointsTable =
                new ObservableCollection<ClubPointsTableRowViewModel>(
                    ClubPointsTable.OrderByDescending(
                        order => order.TotalPoints));
        }
    }
}