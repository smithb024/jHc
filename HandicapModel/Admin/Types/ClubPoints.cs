namespace HandicapModel.Admin.Types
{
    using System.Collections.Generic;

    /// <summary>
    /// Stores the club points whilst the results are being worked out.
    /// </summary>
    public class ClubPoints
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ClubPoints"/> class.
        /// </summary>
        /// <param name="clubName">Name of the club</param>
        /// <param name="teamSize">Team size</param>
        /// <param name="finishingPoints">points scored for finishing</param>
        /// <param name="SBPoints">points scored for a SB</param>
        public ClubPoints(
            string clubName,
            int teamSize,
            int finishingPoints,
            int sbPoints)
        {
            ClubName = clubName;
            TeamSize = teamSize;
            PointsScoredForFinishing = finishingPoints;
            PointsScoredForSB = sbPoints;
            PositionPointsList = new List<int>();
        }

        /// <summary>
        /// Gets the name of the club.
        /// </summary>
        public string ClubName { get; private set; }

        /// <summary>
        /// Gets the finishing points scored for the club.
        /// </summary>
        public int FinishingPoints
        {
            get
            {
                if (PositionPointsList.Count + NumberOfFirstTimers >= TeamSize)
                {
                    return TeamSize * PointsScoredForFinishing;
                }
                else
                {
                    return PositionPointsList.Count * PointsScoredForFinishing + NumberOfFirstTimers * PointsScoredForFinishing;
                }
            }
        }

        /// <summary>
        /// Gets the position points for the club.
        /// </summary>
        public int PositionPoints
        {
            get
            {
                int positionPoints = 0;

                foreach (int points in PositionPointsList)
                {
                    positionPoints += points;
                }

                return positionPoints;
            }
        }

        /// <summary>
        /// Gets the points scored for running season best times.
        /// </summary>
        public int BestPoints
        {
            get { return NumberOfSBs * PointsScoredForSB; }
        }

        /// <summary>
        /// Gets or sets the size of the team.
        /// </summary>
        public int TeamSize { get; set; }

        /// <summary>
        /// Gets or sets the points awarded for positions.
        /// </summary>
        /// <remarks>
        /// Points are stored in an array to count how many have been added.
        /// </remarks>
        private List<int> PositionPointsList { get; set; }

        /// <summary>
        /// Gets or sets the number of first timers for the club.
        /// </summary>
        private int NumberOfFirstTimers { get; set; }

        /// <summary>
        /// Gets or sets the number of season bests for the club.
        /// </summary>
        private int NumberOfSBs { get; set; }

        /// <summary>
        /// Gets or sets points awarded for the SB points.
        /// </summary>
        private int PointsScoredForSB { get; set; }

        /// <summary>
        /// Gets or sets points awared for finshing.
        /// </summary>
        private int PointsScoredForFinishing { get; set; }

        /// <summary>
        /// Adds a new result to the points table.
        /// </summary>
        /// <remarks>
        /// Add the number of first timers, season bests and points (if team size has not been exceeded).
        /// </remarks>
        /// <param name="positionPoints">position points scored</param>
        /// <param name="firstTimer">first timer</param>
        /// <param name="sbAttained">season best run</param>
        public void AddNewResult(int positionPoints,
                                 bool firstTimer,
                                 bool sbAttained)
        {
            if (firstTimer)
            {
                ++NumberOfFirstTimers;
            }
            else
            {
                if (sbAttained)
                {
                    ++NumberOfSBs;
                }

                if (PositionPointsList.Count < TeamSize)
                {
                    PositionPointsList.Add(positionPoints);
                }
            }
        }
    }
}