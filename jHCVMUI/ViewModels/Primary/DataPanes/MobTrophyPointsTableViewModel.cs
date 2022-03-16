namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using HandicapModel.SeasonModel;
    using HandicapModel.Common;
    using jHCVMUI.ViewModels.ViewModels;
    using jHCVMUI.ViewModels.ViewModels.Types;
    using jHCVMUI.ViewModels.ViewModels.Types.Clubs;

    using NynaeveLib.Commands;
    using System;
    using HandicapModel.Interfaces.SeasonModel;

    /// <summary>
    /// View Model used by the Data Pane to display the Mob Trophy points table.
    /// </summary>
    public class MobTrophyPointsTableViewModel : ViewModelBase
    {
        /// <summary>
        /// The Mob Trophy points table.
        /// </summary>
        private ObservableCollection<MobTrophyPointsTableRowViewModel> mobTrophyPointsTable;

        /// <summary>
        /// Index of the currently selected item on the table.
        /// </summary>
        private int currentMobTrophyPointsTableIndex;

        /// <summary>
        /// Indicates whether verbose or concise data is show.
        /// </summary>
        private bool expandedData;

        /// <summary>
        /// The associated season model.
        /// </summary>
        private ISeason model;

        /// <summary>
        /// View model which suports the mob trophy points table.
        /// </summary>
        /// <param name="model">season model</param>
        public MobTrophyPointsTableViewModel(ISeason model)
        {
            this.model = model;
            this.mobTrophyPointsTable = new ObservableCollection<MobTrophyPointsTableRowViewModel>();
            this.currentMobTrophyPointsTableIndex = 0;
            this.expandedData = false;

            this.model.ClubsChangedEvent += this.PopulateMobTrophyPointsData;
            this.PopulateMobTrophyTable();

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
        /// Gets points sets the Mob Trophy points table.
        /// </summary>
        public ObservableCollection<MobTrophyPointsTableRowViewModel> MobTrophyPointsTable
        {
            get
            {
                return this.mobTrophyPointsTable;
            }

            set
            {
                this.mobTrophyPointsTable = value;
                this.RaisePropertyChangedEvent(nameof(this.MobTrophyPointsTable));
            }
        }

        /// <summary>
        /// Gets or sets the index of the Mob Trophy points table
        /// </summary>
        public int SelectedMobTrophyPointsTableIndex
        {
            get
            {
                return this.currentMobTrophyPointsTableIndex;
            }

            set
            {
                this.currentMobTrophyPointsTableIndex = value;
                this.RaisePropertyChangedEvent(nameof(this.SelectedMobTrophyPointsTableIndex));
            }
        }

        /// <summary>
        /// Toggle expanded data flag.
        /// </summary>
        public void UpdateExpandedFlag()
        {
            ExpandedData = !ExpandedData;

            foreach (MobTrophyPointsTableRowViewModel mobTrophyPoints in MobTrophyPointsTable)
            {
                mobTrophyPoints.ExpandedData = ExpandedData;
            }
        }

        /// <summary>
        /// Used to populate the Mob Trophy points table
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        public void PopulateMobTrophyPointsData(
            object sender,
            EventArgs e)
        {
            MobTrophyPointsTable.Clear();
            this.PopulateMobTrophyTable();
        }

        /// <summary>
        /// Calculate and populate the Mob Trophy points table.
        /// </summary>
        private void PopulateMobTrophyTable()
        {
            foreach (ClubSeasonDetails clubSeasonDetail in this.model.Clubs)
            {
                MobTrophyPointsTableRowViewModel mobTrophyPoints =
                    new MobTrophyPointsTableRowViewModel(
                        clubSeasonDetail.Name,
                        clubSeasonDetail.ClubCompetition.TotalPoints,
                        clubSeasonDetail.ClubCompetition.TotalFinishingPoints.ToString(),
                        clubSeasonDetail.ClubCompetition.TotalPositionPoints.ToString(),
                        clubSeasonDetail.ClubCompetition.TotalBestPoints.ToString());

                foreach (CommonPoints eventPoints in clubSeasonDetail.ClubCompetition.Points)
                {
                    mobTrophyPoints.AddPoints(
                        new PointsType(
                            eventPoints.FinishingPoints,
                            eventPoints.PositionPoints,
                            eventPoints.BestPoints,
                            eventPoints.Date));
                }

                MobTrophyPointsTable.Add(mobTrophyPoints);
            }

            MobTrophyPointsTable =
                new ObservableCollection<MobTrophyPointsTableRowViewModel>(
                    MobTrophyPointsTable.OrderByDescending(
                        order => order.TotalPoints));
        }
    }
}