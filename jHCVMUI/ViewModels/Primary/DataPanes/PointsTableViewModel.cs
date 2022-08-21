namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using CommonLib.Types;
    using HandicapModel.AthletesModel;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.SeasonModel;
    using jHCVMUI.ViewModels.ViewModels;
    using jHCVMUI.ViewModels.ViewModels.Types.Athletes;

    /// <summary>
    /// View Model used by the Data Pane to display the athletes points table.
    /// </summary>
    public class PointsTableViewModel : ViewModelBase
    {
        /// <summary>
        /// The handicap model.
        /// </summary>
        private readonly IModel model;

        /// <summary>
        /// Associated season model.
        /// </summary>
        private readonly ISeason seasonModel;

        /// <summary>
        /// Points table.
        /// </summary>
        private ObservableCollection<PointsTableRowViewModel> pointsTable;

        /// <summary>
        /// Initialises a new instance of the <see cref="PointsTableViewModel"/> class.
        /// </summary>
        /// <param name="model">handicap model</param>
        public PointsTableViewModel(
            IModel model)
        {
            this.model = model;
            this.seasonModel = this.model.CurrentSeason;
            this.pointsTable = new ObservableCollection<PointsTableRowViewModel>();

            this.seasonModel.AthleteCollectionChangedEvent += this.RegenerateThePointsTable;
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
                this.RaisePropertyChangedEvent(nameof(this.PointsTable));
            }
        }

        /// <summary>
        /// Clear all existing entries on the points table and generate from scratch.
        /// </summary>
        /// <param name="sender">The season model</param>
        /// <param name="e">event arguments</param>
        public void RegenerateThePointsTable(
            object sender,
            EventArgs e)
        {
            foreach (PointsTableRowViewModel row in this.PointsTable)
            {
                row.Dispose();
            }

            this.PointsTable.Clear();
            this.PopulatePointsTable();
       }

        /// <summary>
        /// Calculate and populate the athletes points table.
        /// </summary>
        private void PopulatePointsTable()
        {
            Athletes athletesModel = this.model.Athletes;

            foreach (IAthleteSeasonDetails athlete in this.seasonModel.Athletes)
            {
                if (athlete.Points.TotalPoints > 0)
                {
                    double averagePoints = 0;
                    if (athlete.NumberOfAppearances > 0)
                    {
                        averagePoints = (double)athlete.Points.TotalPoints / (double)athlete.NumberOfAppearances;
                    }

                    TimeType pb = athletesModel.GetPB(athlete.Key);
                    string runningNumber =
                        athletesModel.GetAthleteRunningNumber(
                            athlete.Key);

                    AthleteDetails athleteModel =
                        athletesModel.GetAthlete(
                            athlete.Key);

                    if (athleteModel == null)
                    {
                        // TODO: Unexpected, it should be logged.
                        continue;
                    }

                    PointsTableRowViewModel newRow =
                        new PointsTableRowViewModel(
                            athlete,
                            athleteModel);

                    this.PointsTable.Add(newRow);
                }
            }

            this.PointsTable =
                new ObservableCollection<PointsTableRowViewModel>(
                    this.PointsTable.OrderByDescending(
                        order => order.Points));
        }
    }
}