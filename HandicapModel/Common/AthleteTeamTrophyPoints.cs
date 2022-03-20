namespace HandicapModel.Common
{
    using CommonLib.Types;
    using Interfaces.Common;

    /// <summary>
    /// A single points entry for an athlete within the Team Trophy.
    /// </summary>
    public class AthleteTeamTrophyPoints : IAthleteTeamTrophyPoints
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="CommonTeamTrophyPoints"/> class.
        /// </summary>
        /// <param name="point">points scored</param>
        /// <param name="date">date the points were scored</param>
        public AthleteTeamTrophyPoints(
            int point,
            DateType date)
        {
            this.Point = point;
            this.Date = date;
        }

        /// <summary>
        /// Gets the number of points.
        /// </summary>
        public int Point { get; }

        /// <summary>
        /// Gets the date that the points were earned.
        /// </summary>
        public DateType Date { get; }
    }
}
