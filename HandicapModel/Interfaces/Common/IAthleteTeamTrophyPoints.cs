namespace HandicapModel.Interfaces.Common
{
    using CommonLib.Types;

    /// <summary>
    /// Interface which describes a single points entry for an athlete within the Team Trophy.
    /// </summary>
    public interface IAthleteTeamTrophyPoints
    {
        /// <summary>
        /// Gets the number of points.
        /// </summary>
        int Point { get; }

        /// <summary>
        /// Gets the date that the points were earned.
        /// </summary>
        DateType Date { get; }
    }
}