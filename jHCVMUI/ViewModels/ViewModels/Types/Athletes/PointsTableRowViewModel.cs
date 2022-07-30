namespace jHCVMUI.ViewModels.ViewModels.Types.Athletes
{
    using CommonLib.Types;

    /// <summary>
    /// Class describing a single entry of the points table
    /// </summary>
    public class PointsTableRowViewModel : AthleteBase
    {
        /// <summary>
        /// Personal best
        /// </summary>
        private string personalBest;

        /// <summary>
        /// Total number of points scored.
        /// </summary>
        private int points;

        /// <summary>
        /// Number of points scored by finishing an event.
        /// </summary>
        private int finishingPoints;

        /// <summary>
        /// Number of points scored by finishing position.
        /// </summary>
        private int positionPoints;

        /// <summary>
        /// Number of point score by improving a time.
        /// </summary>
        private int bestPoints;

        /// <summary>
        /// Number of event started.
        /// </summary>
        private int numberOfRuns;

        /// <summary>
        /// Average number of points scored per event.
        /// </summary>
        private string averagePoints;

        /// <summary>
        /// Season best.
        /// </summary>
        private string seasonBest;

        /// <summary>
        /// Initialises a new instance of the <see cref="PointsTableRowViewModel"/> class.
        /// </summary>
        /// <param name="key">entry key</param>
        /// <param name="name">name of the athlete</param>
        /// <param name="pb">athlete's personal best</param>
        /// <param name="points">total number of points scored</param>
        /// <param name="finishingPoints">number of points scored by finishing an event</param>
        /// <param name="positionPoints">number of points scored through event position</param>
        /// <param name="bestPoints">number of points scored by improving on the season best time</param>
        /// <param name="raceNumber">athlete's number</param>
        /// <param name="numberOfRuns">number of runs started</param>
        /// <param name="averagePoints">average number of points scored per event</param>
        /// <param name="sb">athlete's season best</param>
        public PointsTableRowViewModel(
          int key,
          string name,
          TimeType pb,
          int points,
          int finishingPoints,
          int positionPoints,
          int bestPoints,
          string raceNumber,
          int numberOfRuns,
          string averagePoints,
          TimeType sb)
          : base(key, name)
        {
            this.personalBest = pb.ToString();
            this.points = points;
            this.finishingPoints = finishingPoints;
            this.positionPoints = positionPoints;
            this.bestPoints = bestPoints;
            this.RaceNumber = raceNumber;
            this.numberOfRuns = numberOfRuns;
            this.averagePoints = averagePoints;
            this.seasonBest = sb.ToString();
        }

        /// <summary>
        /// Gets or sets the PB.
        /// </summary>
        public string PB
        {
            get
            {
                return this.personalBest;
            }

            set
            {
                this.personalBest = value;
                this.RaisePropertyChangedEvent("PB");
            }
        }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        public int Points
        {
            get
            {
                return this.points;
            }

            set
            {
                this.points = value;
                this.RaisePropertyChangedEvent("Points");
            }
        }

        /// <summary>
        /// Gets or sets the finishing points.
        /// </summary>
        public int FinishingPoints
        {
            get
            {
                return this.finishingPoints;
            }

            set
            {
                this.finishingPoints = value;
                this.RaisePropertyChangedEvent("FinishingPoints");
            }
        }

        /// <summary>
        /// Gets or sets the position points.
        /// </summary>
        public int PositionPoints
        {
            get
            {
                return this.positionPoints;
            }

            set
            {
                this.positionPoints = value;
                this.RaisePropertyChangedEvent("PositionPoints");
            }
        }

        /// <summary>
        /// Gets or sets the best points.
        /// </summary>
        public int BestPoints
        {
            get
            {
                return this.bestPoints;
            }

            set
            {
                this.bestPoints = value;
                this.RaisePropertyChangedEvent("BestPoints");
            }
        }

        /// <summary>
        /// Gets or sets the race number.
        /// </summary>
        public string RaceNumber { get; }

        /// <summary>
        /// Gets or sets the number of runs.
        /// </summary>
        public int NumberOfRuns
        {
            get
            {
                return this.numberOfRuns;
            }

            set
            {
                this.numberOfRuns = value;
                this.RaisePropertyChangedEvent("NumberOfRuns");
            }
        }

        /// <summary>
        /// Gets or sets the average points.
        /// </summary>
        public string AveragePoints
        {
            get
            {
                return this.averagePoints;
            }

            set
            {
                this.averagePoints = value;
                this.RaisePropertyChangedEvent("AveragePoints");
            }
        }

        /// <summary>
        /// Gets or sets the SB.
        /// </summary>
        public string SB
        {
            get
            {
                return this.seasonBest;
            }

            set
            {
                this.seasonBest = value;
                this.RaisePropertyChangedEvent("SB");
            }
        }
    }
}