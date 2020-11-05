namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using CommonLib.Types;
    using HandicapModel.AthletesModel;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.SeasonModel;
    using jHCVMUI.ViewModels.ViewModels;
    using jHCVMUI.ViewModels.ViewModels.Types.Athletes;

    /// <summary>
    /// View Model used by the Data Pane to display the athletes points table.
    /// </summary>
    public class PointsTableViewModel : ViewModelBase
    {
        /// <summary>
        /// Associated season model.
        /// </summary>
        private ISeason seasonModel;

        /// <summary>
        /// Associated athletes model.
        /// </summary>
        private Athletes athletesModel;

        /// <summary>
        /// Points table.
        /// </summary>
        private ObservableCollection<PointsTableRowViewModel> pointsTable;

        /// <summary>
        /// Initialises a new instance of the <see cref="PointsTableViewModel"/> class.
        /// </summary>
        /// <param name="seasonModel">season model</param>
        /// <param name="athletesModel">athletes model</param>
        public PointsTableViewModel(
            ISeason seasonModel,
            Athletes athletesModel)
        {
            this.seasonModel = seasonModel;
            this.athletesModel = athletesModel;
            this.pointsTable = new ObservableCollection<PointsTableRowViewModel>();

            this.seasonModel.AthletesChangedEvent += this.PopulateAthleteCurrentSeasonData;
            this.PopulatePointsTable();
        }

        /// <summary>
        /// Gets points sets the results table.
        /// </summary>
        public ObservableCollection<PointsTableRowViewModel> PointsTable
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
        /// Populate the points table.
        /// </summary>
        /// <param name="athletes">athletes collection from the model</param>
        public void PopulateAthleteCurrentSeasonData(
            object sender,
            EventArgs e)
        {
            this.PointsTable.Clear();
            this.PopulatePointsTable();

            //foreach (AthleteSeasonDetails athlete in this.seasonModel.Athletes)
            //{
            //    if (athlete.Points.TotalPoints > 0)
            //    {
            //        double averagePoints = 0;
            //        if (athlete.NumberOfAppearances > 0)
            //        {
            //            averagePoints = (double)athlete.Points.TotalPoints / (double)athlete.NumberOfAppearances;
            //        }

            //        TimeType pb = this.athletesModel.GetPB(athlete.Key);
            //        string runningNumber =
            //            this.athletesModel.GetAthleteRunningNumber(
            //                athlete.Key);

            //        this.PointsTable.Add(
            //          new PointsTableRowViewModel(
            //            athlete.Key,
            //            athlete.Name,
            //            pb,
            //            athlete.Points.TotalPoints,
            //            athlete.Points.FinishingPoints,
            //            athlete.Points.PositionPoints,
            //            athlete.Points.BestPoints,
            //            runningNumber,
            //            athlete.NumberOfAppearances,
            //            averagePoints.ToString("0.##"),
            //            athlete.SB));
            //    }
            //}

            //this.PointsTable =
            //    new ObservableCollection<PointsTableRowViewModel>(
            //        PointsTable.OrderByDescending(
            //            order => order.Points));
        }

        /// <summary>
        /// Calculate and populate the athletes points table.
        /// </summary>
        private void PopulatePointsTable()
        {
            foreach (AthleteSeasonDetails athlete in this.seasonModel.Athletes)
            {
                if (athlete.Points.TotalPoints > 0)
                {
                    double averagePoints = 0;
                    if (athlete.NumberOfAppearances > 0)
                    {
                        averagePoints = (double)athlete.Points.TotalPoints / (double)athlete.NumberOfAppearances;
                    }

                    TimeType pb = this.athletesModel.GetPB(athlete.Key);
                    string runningNumber =
                        this.athletesModel.GetAthleteRunningNumber(
                            athlete.Key);

                    this.PointsTable.Add(
                      new PointsTableRowViewModel(
                        athlete.Key,
                        athlete.Name,
                        pb,
                        athlete.Points.TotalPoints,
                        athlete.Points.FinishingPoints,
                        athlete.Points.PositionPoints,
                        athlete.Points.BestPoints,
                        runningNumber,
                        athlete.NumberOfAppearances,
                        averagePoints.ToString("0.##"),
                        athlete.SB));
                }
            }

            this.PointsTable =
                new ObservableCollection<PointsTableRowViewModel>(
                    PointsTable.OrderByDescending(
                        order => order.Points));
        }
    }
}