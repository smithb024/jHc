namespace jHCVMUI.ViewModels.ViewModels.Types.Athletes
{
    using HandicapModel.AthletesModel;
    using HandicapModel.Interfaces.SeasonModel;
    using System;

    /// <summary>
    /// Class describing a single entry of the points table
    /// </summary>
    public class PointsTableRowViewModel : AthleteBase
    {
        /// <summary>
        /// The athlete details model object for the current season.
        /// </summary>
        private readonly IAthleteSeasonDetails athleteSeasonDetails;

        /// <summary>
        /// Get the athlete points model object for the current season.
        /// </summary>
        private readonly IAthleteSeasonPoints athleteSeasonPoints;

        /// <summary>
        /// Gets the global athlete model object;
        /// </summary>
        private readonly AthleteDetails athleteDetails;

        /// <summary>
        /// Initialises a new instance of the <see cref="PointsTableRowViewModel"/> class.
        /// </summary>
        /// <param name="athleteSeasonDetails">
        /// The model object for an athlete in the current season.
        /// </param>
        /// <param name="athleteDetails">
        /// The model object for an athlete. 
        /// </param>
        public PointsTableRowViewModel(
            IAthleteSeasonDetails athleteSeasonDetails,
            AthleteDetails athleteDetails)
          : base(athleteDetails.Key, athleteDetails.Name)
        {
            this.athleteSeasonDetails = athleteSeasonDetails;
            this.athleteSeasonPoints = athleteSeasonDetails.Points;
            this.athleteDetails = athleteDetails;

            this.PB = this.athleteDetails.PB.ToString();
            this.Points = this.athleteSeasonPoints.TotalPoints;
            this.FinishingPoints = this.athleteSeasonPoints.FinishingPoints;
            this.PositionPoints = this.athleteSeasonPoints.PositionPoints;
            this.BestPoints = this.athleteSeasonPoints.BestPoints;
            this.RaceNumber = this.athleteDetails.PrimaryNumber;
            this.NumberOfRuns = this.athleteSeasonDetails.NumberOfAppearances;
            this.SB = this.athleteSeasonDetails.SB.ToString();
            this.Sex = this.athleteDetails.Sex.ToString();
        }

        /// <summary>
        /// Gets the PB.
        /// </summary>
        public string PB { get; }

        /// <summary>
        /// Gets the points.
        /// </summary>
        public int Points { get; }

        /// <summary>
        /// Gets the finishing points.
        /// </summary>
        public int FinishingPoints { get; }

        /// <summary>
        /// Gets the position points.
        /// </summary>
        public int PositionPoints { get; }

        /// <summary>
        /// Gets the best points.
        /// </summary>
        public int BestPoints { get; }

        /// <summary>
        /// Gets the race number.
        /// </summary>
        public string RaceNumber { get; }

        /// <summary>
        /// Gets the number of runs.
        /// </summary>
        public int NumberOfRuns { get; }

        /// <summary>
        /// Gets or sets the average points.
        /// </summary>
        public string AveragePoints
        {
            get
            {
                double averagePoints = 0;
                if (this.NumberOfRuns > 0)
                {
                    averagePoints = (double)this.Points / this.NumberOfRuns;
                }


                return averagePoints.ToString("0.##");
            }
        }

        /// <summary>
        /// Gets  the SB.
        /// </summary>
        public string SB { get; }

        /// <summary>
        /// Gets the Sex of the athlete.
        /// </summary>
        public string Sex { get; }
    }
}