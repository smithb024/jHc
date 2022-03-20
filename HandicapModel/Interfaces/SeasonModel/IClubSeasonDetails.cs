namespace HandicapModel.Interfaces.SeasonModel
{
    using HandicapModel.Interfaces.Common;
    using CommonLib.Types;

    /// <summary>
    /// Interface which describes a season for a specific club. It manages the various competetions.
    /// </summary>
    public interface IClubSeasonDetails
    {
        /// <summary>
        /// Gets the club name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the details of the mob trophy for this team.
        /// </summary>
        IMobTrophy MobTrophy { get; }

        /// <summary>
        /// Gets the details of the team Trophy for this team.
        /// </summary>
        ITeamTrophyCompetition TeamTrophy { get; }

        /// <summary>
        /// Add a new event.
        /// </summary>
        /// <param name="newPoints">points received</param>
        void AddNewEvent(
            ICommonPoints newPoints);

        /// <summary>
        /// Add a new event.
        /// </summary>
        /// <param name="newEvent">new event</param>
        void AddNewEvent(
            ITeamTrophyEvent newEvent);

        /// <summary>
        /// Set points to an existing entry
        /// </summary>
        /// <param name="eventIndex">event index</param>
        /// <param name="updatedPoints">points received</param>
        void SetMobTrophyPoints(
            int eventIndex,
            ICommonPoints updatedPoints);

        /// <summary>
        /// Remove all points corresponding to the argument date. Do nothing if there are no points for the date.
        /// </summary>
        /// <param name="date"></param>
        void RemoveMobTrophyPoints(DateType date);
    }
}
